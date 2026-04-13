using System.Net.Http.Headers;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HouseBroker.Test.ApiTests.APITestHelper;

public class ApiTestCaseBase : IDisposable
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly IServiceProvider Services;
    protected readonly Mock<ICacheService> CacheServiceMock = new();

    public ApiTestCaseBase()
    {
        var dbName = $"ApiTestDb_{Guid.NewGuid()}";
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = "Server=none;Database=none;User Id=none;Password=none;Encrypt=false;",
                    ["ConnectionStrings:Redis"] = "localhost:6379",
                    ["JwtSettings:SecretKey"] = "SuperSecretKeyForTestingPurposes123!",
                    ["JwtSettings:Issuer"] = "HouseBroker",
                    ["JwtSettings:Audience"] = "HouseBrokerUser",
                    ["JwtSettings:ExpiryMinutes"] = "60"
                });
            });

            builder.ConfigureTestServices(services =>
            {
                // DB - Remove any existing DbContext registration including those from Identity
                var dbContextDescriptors = services.Where(d => 
                    d.ServiceType == typeof(HouseBrokerDbContext) || 
                    d.ServiceType == typeof(DbContextOptions<HouseBrokerDbContext>) ||
                    d.ServiceType.Name.Contains("DbContextOptions")).ToList();
                
                foreach (var descriptor in dbContextDescriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HouseBrokerDbContext>(options => 
                {
                    options.UseInMemoryDatabase(dbName);
                });

                // Cache
                services.AddDistributedMemoryCache(); // Satisfy IDistributedCache
                var cacheDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICacheService));
                if (cacheDescriptor != null) services.Remove(cacheDescriptor);
                services.AddSingleton(CacheServiceMock.Object);

                // Mock Auth
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        });

        Client = Factory.CreateClient();
        Services = Factory.Services;
    }

    protected void AuthenticateAs(long userId, string role = "Admin")
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        TestAuthHandler.UserId = userId;
        TestAuthHandler.UserRole = role;
    }

    public void Dispose()
    {
        Factory.Dispose();
        Client.Dispose();
    }
}


