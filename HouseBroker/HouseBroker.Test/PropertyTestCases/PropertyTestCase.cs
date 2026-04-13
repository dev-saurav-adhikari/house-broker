using HouseBroker.Infrastructure.Services;
using HouseBroker.Application.Constants;
using HouseBroker.Application.CustomException;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Enums;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Test.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace HouseBroker.Test.PropertyTestCases;

public class PropertyTestCase : IClassFixture<ServiceCollectionFixture>
{
    private readonly ServiceCollectionFixture _fixture;
    private readonly IPropertyService _service;
    private readonly HouseBrokerDbContext _dbContext;
    private readonly UserManager<IdentityUser<long>> _userManager;

    public PropertyTestCase(ServiceCollectionFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IPropertyService>();
        _dbContext = _fixture.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();
        _userManager = _fixture.ServiceProvider.GetRequiredService<UserManager<IdentityUser<long>>>();

        // ensure each test runs with a fresh database for isolation
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _dbContext.ChangeTracker.Clear();
        
        _fixture.FileServiceMock.Invocations.Clear();
        _fixture.CommissionServiceMock.Invocations.Clear();
        _fixture.CacheServiceMock.Invocations.Clear();

        // set up default mock behavior to avoid null reference issues in tests
        _fixture.FileServiceMock.Setup(f => f.SaveFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(string.Empty);
    }

    [Fact]
    public async Task GetAllPropertiesAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        var brokerId = 100L;
        var broker = new IdentityUser<long> { Id = brokerId, Email = "broker@test.com", UserName = "broker@test.com", PhoneNumber = "123456" };
        await _userManager.CreateAsync(broker);

        var properties = new List<Property>
        {
            new() { Id = 1, Title = "Luxury Villa", Description = "A beautiful villa", Price = 1000000, ProvinceId = 1, DistrictId = 1, BrokerId = brokerId, IsAvailable = true, ImageUrl = "", PropertyType = PropertyTypeEnum.House },
            new() { Id = 2, Title = "Cheap Apartment", Description = "Old but gold", Price = 500000, ProvinceId = 2, DistrictId = 2, BrokerId = brokerId, IsAvailable = true, ImageUrl = "", PropertyType = PropertyTypeEnum.Apartment },
            new() { Id = 3, Title = "Office Space", Description = "Modern office", Price = 2000000, ProvinceId = 1, DistrictId = 1, BrokerId = brokerId, IsAvailable = false, ImageUrl = "", PropertyType = PropertyTypeEnum.Land }
        };
        _dbContext.Properties.AddRange(properties);
        await _dbContext.SaveChangesAsync();

        _fixture.CacheServiceMock.Setup(c => c.GetOrSetVersionAsync(It.IsAny<string>())).ReturnsAsync(1);
        _fixture.CacheServiceMock.Setup(c => c.GetAsync<Pagination<PropertyDetailWithBrokerInfoDto>>(It.IsAny<string>()))
            .ReturnsAsync((Pagination<PropertyDetailWithBrokerInfoDto>?)null);

        // Act and filter by search and province
        var filter1 = new PropertyFilterDto { Search = "Villa", ProvinceId = 1, PageNumber = 1, PageSize = 10 };
        var result1 = await _service.GetAllPropertiesAsync(filter1);
        result1.Items.Count.ShouldBe(1);
        result1.Items[0].Title.ShouldBe("Luxury Villa");

        // Act and filter by price range
        var filter2 = new PropertyFilterDto { MinPrice = 400000, MaxPrice = 600000, PageNumber = 1, PageSize = 10 };
        var result2 = await _service.GetAllPropertiesAsync(filter2);
        result2.Items.Count.ShouldBe(1);
        result2.Items[0].Title.ShouldBe("Cheap Apartment");

        // Act and filter by property type
        var filter3 = new PropertyFilterDto { PropertyType = PropertyTypeEnum.Apartment, PageNumber = 1, PageSize = 10 };
        var result3 = await _service.GetAllPropertiesAsync(filter3);
        result3.Items.Count.ShouldBe(1);
        result3.Items[0].PropertyType.ShouldBe(PropertyTypeEnum.Apartment);
    }

    [Fact]
    public async Task GetAllPropertiesAsync_Pagination_ShouldWorkCorrectly()
    {
        // Arrange
        var brokerId = 100L;
        var broker = new IdentityUser<long> { Id = brokerId, Email = "broker@test.com", UserName = "broker@test.com" };
        await _userManager.CreateAsync(broker);

        for (int i = 1; i <= 15; i++)
        {
            _dbContext.Properties.Add(new Property { Id = i, Title = $"Prop {i}", Description = "Desc", Price = 1000 * i, BrokerId = brokerId, IsAvailable = true, ImageUrl = "" });
        }
        await _dbContext.SaveChangesAsync();

        _fixture.CacheServiceMock.Setup(c => c.GetOrSetVersionAsync(It.IsAny<string>())).ReturnsAsync(1);

        // Act
        var filter = new PropertyFilterDto { PageNumber = 2, PageSize = 10 };
        var result = await _service.GetAllPropertiesAsync(filter);

        // Assert
        result.Items.Count.ShouldBe(5);
        result.TotalCount.ShouldBe(15);
        result.CurrentPage.ShouldBe(2);
    }

    [Fact]
    public async Task InsertProperty_ShouldSaveFullDetails()
    {
        // Arrange
        var userId = 100L;
        var dto = new InsertPropertyDetailDto
        {
            Title = "Full Detail House",
            Description = "A perfect home",
            Price = 2500000,
            ProvinceId = 3,
            DistrictId = 23,
            Municipality = "Kathmandu",
            WardNumber = 4,
            LandMark = "Near Mall",
            PropertyType = PropertyTypeEnum.House,
            ImageFile = Mock.Of<IFormFile>()
        };

        _fixture.FileServiceMock.Setup(f => f.SaveFileAsync(It.IsAny<IFormFile>())).ReturnsAsync("path_to_image.jpg");
        _fixture.CommissionServiceMock.Setup(c => c.CalculateCommissionAsync(dto.Price)).ReturnsAsync(50000m);

        // Act
        await _service.InsertProperty(dto, userId);

        // Assert
        var property = _dbContext.Properties.FirstOrDefault(p => p.Title == "Full Detail House");
        property.ShouldNotBeNull();
        property.Municipality.ShouldBe("Kathmandu");
        property.WardNumber.ShouldBe(4);
        property.EstimatedCommission.ShouldBe(50000m);
    }
    
    [Fact]
    public async Task InsertProperty_ShouldSave_Without_Image()
    {
        // Arrange
        var userId = 100L;
        var dto = new InsertPropertyDetailDto
        {
            Title = "Full Detail House",
            Description = "A perfect home",
            Price = 2500000,
            ProvinceId = 3,
            DistrictId = 23,
            Municipality = "Kathmandu",
            WardNumber = 4,
            LandMark = "Near Mall",
            PropertyType = PropertyTypeEnum.House,
            ImageFile = null
        };
        
        _fixture.CommissionServiceMock.Setup(c => c.CalculateCommissionAsync(dto.Price)).ReturnsAsync(50000m);

        // Act
        await _service.InsertProperty(dto, userId);

        // Assert
        var property = _dbContext.Properties.FirstOrDefault(p => p.Title == "Full Detail House");
        property.ShouldNotBeNull();
        property.Municipality.ShouldBe("Kathmandu");
        property.WardNumber.ShouldBe(4);
        property.EstimatedCommission.ShouldBe(50000m);
    }

    [Fact]
    public async Task UpdateProperty_ShouldApplyPartialChanges_WithoutImage()
    {
        // Arrange
        var userId = 100L;
        var property = new Property { Id = 1, Title = "Old Title", Description = "Old Desc", Price = 1000000, BrokerId = userId, IsAvailable = true, ImageUrl = "old_img.jpg" };
        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        var updateDto = new UpdatePropertyDto { Title = "New Title", Price = 1200000 };
        _fixture.CommissionServiceMock.Setup(c => c.CalculateCommissionAsync(It.IsAny<decimal>())).ReturnsAsync(12000m);

        // Act
        await _service.UpdateProperty(1, updateDto, userId);

        // Assert
        var updated = _dbContext.Properties.Find(1L);
        updated!.Title.ShouldBe("New Title");
        updated.Price.ShouldBe(1200000);
        updated.ImageUrl.ShouldBe("old_img.jpg"); // image should remain same as pervious if not provided
    }

    [Fact]
    public async Task UpdateProperty_Security_ShouldThrowExceptions()
    {
        // Arrange
        var ownerId = 100L;
        var intruderId = 101L;
        _dbContext.Properties.Add(new Property { Id = 5, Title = "Secured", Description = "Secured Desc", BrokerId = ownerId, IsAvailable = true, ImageUrl = "" });
        await _dbContext.SaveChangesAsync();

        // Act and assert : Unauthorized
        await Should.ThrowAsync<UnauthorizedAccessException>(() => 
            _service.UpdateProperty(5, new UpdatePropertyDto(), intruderId));

        // Act and assert : NotFound
        await Should.ThrowAsync<NotFoundException>(() => 
            _service.UpdateProperty(999, new UpdatePropertyDto(), ownerId));
    }

    [Fact]
    public async Task DeleteProperty_ShouldCleanUpCorrectly()
    {
        // Arrange
        var userId = 100L;
        var property = new Property { Id = 10, Title = "Cleanup", Description = "Cleanup Desc", ImageUrl = "temp/image.jpg", BrokerId = userId, IsAvailable = true };
        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.DeleteProperty(10, userId);

        // Assert
        _dbContext.Properties.Find(10L).ShouldBeNull();
        _fixture.FileServiceMock.Verify(f => f.RemoveFile("temp/image.jpg"), Times.Once);
    }
}