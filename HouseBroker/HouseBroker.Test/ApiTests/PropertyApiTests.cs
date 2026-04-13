using System.Net;
using System.Net.Http.Json;
using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Enums;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Test.ApiTests.APITestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace HouseBroker.Test.ApiTests;

public class PropertyApiTests : ApiTestCaseBase
{
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/Property");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_MissingFields_ShouldReturnBadRequest()
    {
        // Arrange
        AuthenticateAs(1);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(""), "Title"); // Invalid Empty Title

        // Act
        var response = await Client.PostAsync("/api/Property", content);

        // Assert
        // In .NET API controllers with [ApiController], 400 is returned automatically for invalid model state
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Valid_ShouldReturnOk()
    {
        // Arrange
        AuthenticateAs(100);
        
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("API House"), "Title");
        content.Add(new StringContent("Desc"), "Description");
        content.Add(new StringContent("1000000"), "Price");
        content.Add(new StringContent("1"), "ProvinceId");
        content.Add(new StringContent("1"), "DistrictId");
        content.Add(new StringContent("Kathmandu"), "Municipality");
        content.Add(new StringContent("5"), "WardNumber");
        content.Add(new StringContent("1"), "PropertyType"); // 1 = House

        // Mock File
        var fileContent = new ByteArrayContent(new byte[] { 0x1, 0x2 });
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "ImageFile", "house.jpg");

        // Act
        var response = await Client.PostAsync("/api/Property", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // Verify in DB
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
        db.Properties.Any(p => p.Title == "API House").ShouldBeTrue();
    }
}
