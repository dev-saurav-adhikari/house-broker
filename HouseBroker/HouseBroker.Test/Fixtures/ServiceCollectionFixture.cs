using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Infrastructure.Repositories;
using HouseBroker.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
namespace HouseBroker.Test.Fixtures;

public class ServiceCollectionFixture : IAsyncDisposable
{
    private readonly ServiceProvider? _serviceProvider;
    
    public IServiceProvider ServiceProvider => _serviceProvider ?? throw new InvalidOperationException("ServiceProvider not initialized");

    // Mocks for non-database dependencies
    public Mock<ICommissionService> CommissionServiceMock { get; }
    public Mock<ICacheService> CacheServiceMock { get; }
    public Mock<IFileService> FileServiceMock { get; }
    public Mock<IPropertyService> PropertyServiceMock { get; }
    public Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public ServiceCollectionFixture()
    {
        var databaseName = $"TestDatabase_{Guid.NewGuid()}";
        
        CommissionServiceMock = new Mock<ICommissionService>();
        CacheServiceMock = new Mock<ICacheService>();
        FileServiceMock = new Mock<IFileService>();
        PropertyServiceMock = new Mock<IPropertyService>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();

        var services = new ServiceCollection();
        
        // database setup
        services.AddDbContext<HouseBrokerDbContext>(options => 
            options.UseInMemoryDatabase(databaseName));

        // identity setup
        services.AddIdentity<IdentityUser<long>, IdentityRole<long>>()
            .AddEntityFrameworkStores<HouseBrokerDbContext>()
            .AddDefaultTokenProviders();

        services.AddLogging();
        services.AddHttpContextAccessor();

        // register mocks as services
        services.AddSingleton(CacheServiceMock.Object);
        services.AddSingleton(FileServiceMock.Object);
        
        // repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<ICommissionRepository, CommissionRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        
        // services
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<ICommissionService, CommissionService>();

        _serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = false
        });

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
        db.Database.EnsureCreated();
    }

    public T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public async ValueTask DisposeAsync()
    {
        if (_serviceProvider != null)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
            await db.Database.EnsureDeletedAsync();
            await _serviceProvider.DisposeAsync();
        }
    }
}