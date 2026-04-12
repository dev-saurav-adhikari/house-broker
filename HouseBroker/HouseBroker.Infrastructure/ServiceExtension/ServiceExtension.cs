using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Infrastructure.Repositories;
using HouseBroker.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HouseBroker.Infrastructure.ServiceExtension;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region  add repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        
        #endregion
        
        #region add services
        services.AddScoped<IAuthService, AuthService>();

        #endregion
    }

    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
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
    
}