using System;
using System.Security.Claims;
using System.Text;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Application.Settings;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Infrastructure.Repositories;
using HouseBroker.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HouseBroker.Infrastructure.ServiceExtension;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region add repositories

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICommissionRepository, CommissionRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();

        #endregion

        #region add services

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IFileService, FileService>();

        #endregion
    }

    public static void IdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser<long>, IdentityRole<long>>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<HouseBrokerDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HouseBrokerDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                p => p.MigrationsAssembly("HouseBroker.Infrastructure")));
    }

    public static void JwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        // Register JWT authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
            });
    }
}