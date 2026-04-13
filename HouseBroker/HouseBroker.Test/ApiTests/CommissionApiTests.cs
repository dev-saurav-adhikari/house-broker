using System.Net;
using System.Net.Http.Json;
using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Test.ApiTests.APITestHelper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace HouseBroker.Test.ApiTests;

public class CommissionApiTests : ApiTestCaseBase
{
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/Commission");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<APIResponse>();
        content.ShouldNotBeNull();
    }

    [Fact]
    public async Task Create_AsAuthorized_ShouldReturnOk()
    {
        // Arrange
        AuthenticateAs(1, "Admin");
        var dto = new UpsertCommissionDto { MinimumAmount = 0, MaximumAmount = 1000, Rate = 5 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Commission", dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // Verify in DB
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
        db.CommissionSettings.Any(c => c.Rate == 5).ShouldBeTrue();
    }

    [Fact]
    public async Task Create_AsAnonymous_ShouldReturnUnauthorized()
    {
        // Arrange
        var dto = new UpsertCommissionDto { MinimumAmount = 0, MaximumAmount = 1000, Rate = 5 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Commission", dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_Existing_ShouldReturnOk()
    {
        // Arrange
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
            db.CommissionSettings.Add(new CommissionSetting { Id = 1, MinimumAmount = 0, MaximumAmount = 100, Rate = 1 });
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();
        }
        AuthenticateAs(1);

        // Act
        var response = await Client.DeleteAsync("/api/Commission/1");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
