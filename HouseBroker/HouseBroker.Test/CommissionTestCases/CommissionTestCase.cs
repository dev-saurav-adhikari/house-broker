using HouseBroker.Application.Constants;
using HouseBroker.Application.CustomException;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace HouseBroker.Test.CommissionTestCases;

public class CommissionTestCase : IClassFixture<ServiceCollectionFixture>
{
    private readonly ServiceCollectionFixture _fixture;
    private readonly ICommissionService _service;
    private readonly HouseBrokerDbContext _dbContext;

    public CommissionTestCase(ServiceCollectionFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<ICommissionService>();
        _dbContext = _fixture.ServiceProvider.GetRequiredService<HouseBrokerDbContext>();

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _dbContext.ChangeTracker.Clear();
        
        _fixture.CacheServiceMock.Invocations.Clear();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFromCache_IfAvailable()
    {
        // Arrange
        var cachedData = new List<CommissionDto> { new() { Id = 1, Rate = 5 } };
        _fixture.CacheServiceMock.Setup(c => c.GetAsync<List<CommissionDto>>(CacheKeys.CommissionsKey))
            .ReturnsAsync(cachedData);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Count.ShouldBe(1);
        result[0].Rate.ShouldBe(5);
        _fixture.CacheServiceMock.Verify(c => c.GetAsync<List<CommissionDto>>(CacheKeys.CommissionsKey), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenFound()
    {
        // Arrange
        var setting = new CommissionSetting { Id = 100, MinimumAmount = 0, MaximumAmount = 1000, Rate = 2 };
        _dbContext.CommissionSettings.Add(setting);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(100);

        // Assert
        result.ShouldNotBeNull();
        result.Rate.ShouldBe(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldSaveAndInvalidateCache()
    {
        // Arrange
        var dto = new UpsertCommissionDto { MinimumAmount = 0, MaximumAmount = 1000000, Rate = 2 };
        var userId = 1L;

        // Act
        await _service.CreateAsync(dto, userId);

        // Assert
        var saved = _dbContext.CommissionSettings.FirstOrDefault(c => c.Rate == 2);
        saved.ShouldNotBeNull();
        _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.CommissionsKey), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        // Arrange
        var setting = new CommissionSetting { Id = 5, MinimumAmount = 0, MaximumAmount = 1000, Rate = 2 };
        _dbContext.CommissionSettings.Add(setting);
        await _dbContext.SaveChangesAsync();

        var updateDto = new UpsertCommissionDto { MinimumAmount = 0, MaximumAmount = 2000, Rate = 3 };

        // Act
        await _service.UpdateAsync(5, updateDto, 1L);

        // Assert
        var updated = _dbContext.CommissionSettings.Find(5L);
        updated!.Rate.ShouldBe(3);
        updated.MaximumAmount.ShouldBe(2000);
        _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.CommissionsKey), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistent_ShouldThrowNotFound()
    {
        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => 
            _service.UpdateAsync(999, new UpsertCommissionDto(), 1L));
    }

    [Fact]
    public async Task CalculateCommissionAsync_Boundary_LowTier_ShouldReturn2Percent()
    {
        // Arrange (0-1M: 2%)
        SetupTiers();
        
        // Act & Assert
        (await _service.CalculateCommissionAsync(500000)).ShouldBe(10000);
        (await _service.CalculateCommissionAsync(1000000)).ShouldBe(20000);
    }

    [Fact]
    public async Task CalculateCommissionAsync_Boundary_MidTier_ShouldReturn3Percent()
    {
        // Arrange (1M-3M: 3%)
        SetupTiers();

        // Act & Assert
        (await _service.CalculateCommissionAsync(1000001)).ShouldBe(30000.03m);
        (await _service.CalculateCommissionAsync(1500000)).ShouldBe(45000);
        (await _service.CalculateCommissionAsync(3000000)).ShouldBe(90000);
    }

    [Fact]
    public async Task CalculateCommissionAsync_Boundary_HighTier_ShouldReturn5Percent()
    {
        // Arrange (>3M: 5%)
        SetupTiers();

        // Act & Assert
        (await _service.CalculateCommissionAsync(3000001)).ShouldBe(150000.05m);
    }

    [Fact]
    public async Task CalculateCommissionAsync_Rounding_ShouldRoundUp()
    {
        // Arrange (fixed 2% for rounding test)
        var tiers = new List<CommissionSetting> { new() { MinimumAmount = 0, MaximumAmount = 0, Rate = 2 } };
        _fixture.CacheServiceMock.Setup(c => c.GetAsync<List<CommissionSetting>>(CacheKeys.CommissionsKey))
            .ReturnsAsync(tiers);

        // Act & Assert
        (await _service.CalculateCommissionAsync(100.25m)).ShouldBe(2.01m); 
    }

    [Fact]
    public async Task CalculateCommissionAsync_Rounding_ShouldRoundDown()
    {
        // Arrange (fixed 2% for rounding test)
        var tiers = new List<CommissionSetting> { new() { MinimumAmount = 0, MaximumAmount = 0, Rate = 2 } };
        _fixture.CacheServiceMock.Setup(c => c.GetAsync<List<CommissionSetting>>(CacheKeys.CommissionsKey))
            .ReturnsAsync(tiers);

        // Act & Assert
        (await _service.CalculateCommissionAsync(100.125m)).ShouldBe(2m); 
    }

  

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAndInvalidateCache()
    {
        // Arrange
        var setting = new CommissionSetting { Id = 10, MinimumAmount = 0, MaximumAmount = 100, Rate = 1 };
        _dbContext.CommissionSettings.Add(setting);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.DeleteAsync(10);

        // Assert
        _dbContext.CommissionSettings.Find(10L).ShouldBeNull();
        _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.CommissionsKey), Times.Once);
    }
    private void SetupTiers()
    {
        var tiers = new List<CommissionSetting>
        {
            new() { MinimumAmount = 0, MaximumAmount = 1000000, Rate = 2 },
            new() { MinimumAmount = 1000000, MaximumAmount = 3000000, Rate = 3 },
            new() { MinimumAmount = 3000000, MaximumAmount = 0, Rate = 5 } 
        };
        _fixture.CacheServiceMock.Setup(c => c.GetAsync<List<CommissionSetting>>(CacheKeys.CommissionsKey))
            .ReturnsAsync(tiers);
    }
}
