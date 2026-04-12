using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class PropertyService(
    IPropertyRepository _propertyRepository,
    UserManager<IdentityUser<long>> _userManager,
    IFileService _fileService, 
    IHttpContextAccessor _httpContextAccessor) : IPropertyService

    
{
    public async Task<List<PropertyDto>> GetAllProperties()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        // fetch all available the properties
        var allProperties = _propertyRepository.FindByCondition(p => p.IsAvailable);

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
            }).ToListAsync();
    }

    public async Task InsertProperty(InsertPropertyDto property, long userId)
    {
        // upload file 
        string imageUrl = await _fileService.SaveFileAsync(property.ImageFile);
        
        // add property
        await _propertyRepository.AddAsync(new Property
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
            IsAvailable = true
        });
        await _propertyRepository.SaveChangesAsync();
    }
}