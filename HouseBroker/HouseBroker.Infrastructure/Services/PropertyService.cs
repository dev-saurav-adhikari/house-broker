using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class PropertyService( 
    IUnitOfWork _unitOfWork,
    UserManager<IdentityUser<long>> _userManager,
    IFileService _fileService, 
    IHttpContextAccessor _httpContextAccessor) : IPropertyService

    
{
    public async Task<List<PropertyDto>> GetAllProperties()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        // fetch all available the properties
        var allProperties = _unitOfWork.PropertyRepository.FindByCondition(p => p.IsAvailable);

        // select distinct broker id from available properties
        var brokerIds = allProperties.Select(p => p.BrokerId).Distinct();

        // fetch detail information of broker
        var brokers = _userManager.Users
            .Where(u => brokerIds.Contains(u.Id))
            .Select(p => new { p.Id, p.Email, p.PhoneNumber });

        return await (from p in allProperties
            join r in brokers on p.BrokerId equals r.Id
            select new PropertyDto
            {
                Id = p.Id,
                Description = p.Description,
                PropertyType = p.PropertyType,
                Price = p.Price,
                ImageUrl = string.IsNullOrEmpty(p.ImageUrl) 
                    ? null 
                    : $"{baseUrl}{p.ImageUrl}",
                ProvinceId = p.ProvinceId,
                Municipality = p.Municipality,
                WardNumber = p.WardNumber,
                DistrictId = p.DistrictId,
                LandMark = p.LandMark,
                Title = p.Title,
                BrokerId = p.BrokerId,
                BrokerName = r.Email,
                EstimatedCommission =  p.EstimatedCommission,
            }).ToListAsync();
    }

    public async Task InsertProperty(InsertPropertyDto property, long userId)
    {
        // upload file 
        string imageUrl = await _fileService.SaveFileAsync(property.ImageFile);
        
        // add property
        var newProperty = new Property
        {
            Description = property.Description,
            PropertyType = property.PropertyType,
            Price = property.Price,
            ImageUrl = imageUrl,
            ProvinceId = property.ProvinceId,
            Municipality = property.Municipality,
            WardNumber = property.WardNumber,
            DistrictId = property.DistrictId,
            LandMark = property.LandMark,
            Title = property.Title,
            CreatedBy = userId,
            BrokerId = userId,
            IsAvailable = true,
            EstimatedCommission =  await CommissionCalculation(property.Price)
            
        };
        await _unitOfWork.PropertyRepository.AddAsync(newProperty);
        await _unitOfWork.PropertyRepository.SaveChangesAsync();
    }

    private async Task<decimal> CommissionCalculation(decimal price)
    {
        
        if (price <= 0) return 0;

        var tiers = await _unitOfWork.CommissionRepository.GetAll();

        // Find the tier where Price is GREATER than the Min and LESS THAN OR EQUAL to the Max
        var matchingTier = tiers.FirstOrDefault(t => 
            (price > t.MinimumAmount || (t.MinimumAmount == 0 && price >= 0)) && 
            (t.MaximumAmount <= 0 || price <= (decimal)t.MaximumAmount)); // Note the <= here

        if (matchingTier == null) return 0;

        var commission = price * (matchingTier.Rate / 100);
        return Math.Round(commission, 2, MidpointRounding.AwayFromZero);
    }
}