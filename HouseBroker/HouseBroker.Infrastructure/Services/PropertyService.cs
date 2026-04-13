using HouseBroker.Application.CustomException;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HouseBroker.Application.Constants;

namespace HouseBroker.Infrastructure.Services;

public class PropertyService(
    IUnitOfWork _unitOfWork,
    UserManager<IdentityUser<long>> _userManager,
    ICacheService _cacheService,
    ICommissionService _commissionService,
    IFileService _fileService) : IPropertyService
{
    public async Task<Pagination<PropertyDetailWithBrokerInfoDto>> GetAllPropertiesAsync(PropertyFilterDto filter)
    {
        // get current version
        int version = await _cacheService.GetOrSetVersionAsync(CacheKeys.PropertiesVersionKey);
        
        // generate versioned cache key
        string cacheKey = CacheKeys.AllProperties(version, filter);
        
        // check cache
        var cachedProperties = await _cacheService.GetAsync<Pagination<PropertyDetailWithBrokerInfoDto>>(cacheKey);
        
        if (cachedProperties != null)
            return cachedProperties;

        // fetch all available the properties
        var allProperties = _unitOfWork.PropertyRepository.FindByCondition(p => p.IsAvailable &&
            (string.IsNullOrWhiteSpace(filter.Search) || p.Title.Contains(filter.Search) ||
             p.Description.Contains(filter.Search))
            && (filter.ProvinceId == null || filter.ProvinceId == 0 || p.ProvinceId == filter.ProvinceId)
            && (filter.DistrictId == null || filter.DistrictId == 0 || p.DistrictId == filter.DistrictId)
            && (filter.WardNumber == null || filter.WardNumber == 0 || p.WardNumber == filter.WardNumber)
            && (filter.MinPrice == null || filter.MinPrice == 0 || p.Price >= filter.MinPrice)
            && (filter.MaxPrice == null || filter.MaxPrice == 0 || p.Price <= filter.MaxPrice)
            && (filter.PropertyType == null || p.PropertyType == filter.PropertyType));
        
        // query execution and pagination (fetch properties first)
        var propertyPagination = new Pagination<Property>(allProperties, filter.PageNumber, filter.PageSize);
        
        // select distinct broker ids from the current page
        var currentBrokerIds = propertyPagination.Items.Select(p => p.BrokerId).Distinct().ToList();

        // fetch detail information of broker for current page only
        var brokers = await _userManager.Users
            .Where(u => currentBrokerIds.Contains(u.Id))
            .Select(p => new { p.Id, p.Email, p.PhoneNumber })
            .ToListAsync();

        // combine property and broker information
        var resultItems = propertyPagination.Items.Select(p =>
        {
            var r = brokers.FirstOrDefault(b => b.Id == p.BrokerId);
            return new PropertyDetailWithBrokerInfoDto
            {
                Id = p.Id,
                Description = p.Description,
                PropertyType = p.PropertyType,
                Price = p.Price,
                ImageUrl = string.IsNullOrEmpty(p.ImageUrl)
                    ? null
                    : _fileService.GetFileUrl(p.ImageUrl),
                ProvinceId = p.ProvinceId,
                Municipality = p.Municipality,
                WardNumber = p.WardNumber,
                DistrictId = p.DistrictId,
                LandMark = p.LandMark,
                Title = p.Title,
                BrokerId = p.BrokerId,
                BrokerName = r?.Email ?? "Unknown",
                EstimatedCommission = p.EstimatedCommission,
                BrokerEmail = r?.Email ?? string.Empty,
                BrokerPhone = r?.PhoneNumber ?? string.Empty,
            };
        }).ToList();

        var pagination = new Pagination<PropertyDetailWithBrokerInfoDto>(resultItems, propertyPagination.TotalCount, 
            propertyPagination.CurrentPage, propertyPagination.PageSize);
        
        // store under the versioned key (expires in 10 mins, old versions naturally expire too)
        await _cacheService.SetAsync(cacheKey, pagination, TimeSpan.FromMinutes(10));
        
        return pagination;
    }

    public async Task InsertProperty(InsertPropertyDetailDto propertyDetail, long userId)
    {
        var isLocationDetailValid = _unitOfWork.DistrictRepository.FindByCondition(p =>
            p.Id == propertyDetail.DistrictId && p.ProvinceId == propertyDetail.ProvinceId).Any();
        if (!isLocationDetailValid) throw new BadRequestException("Invalid province and district information!");
        // upload file 
        string imageUrl = await _fileService.SaveFileAsync(propertyDetail.ImageFile);

        // add property
        var newProperty = new Property
        {
            Description = propertyDetail.Description,
            PropertyType = propertyDetail.PropertyType,
            Price = propertyDetail.Price,
            ImageUrl = imageUrl,
            ProvinceId = propertyDetail.ProvinceId,
            Municipality = propertyDetail.Municipality,
            WardNumber = propertyDetail.WardNumber,
            DistrictId = propertyDetail.DistrictId,
            LandMark = propertyDetail.LandMark,
            Title = propertyDetail.Title,
            CreatedBy = userId,
            BrokerId = userId,
            IsAvailable = true,
            EstimatedCommission = await _commissionService.CalculateCommissionAsync(propertyDetail.Price)
        };
        // adding new property
        await _unitOfWork.PropertyRepository.AddAsync(newProperty);
        await _unitOfWork.PropertyRepository.SaveChangesAsync();
        
        // Invalidate caches
        await _cacheService.RemoveAsync(CacheKeys.BrokerProperties(userId));
        await _cacheService.IncrementVersionAsync(CacheKeys.PropertiesVersionKey);
    }

    public async Task UpdateProperty(long id, UpdatePropertyDto propertyDto, long userId)
    {
        var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
        if (property == null) throw new NotFoundException("Property not found");
        if (property.BrokerId != userId) throw new UnauthorizedAccessException("You are not authorized to update this property");
        
        
        // data mapping 
        property.Title = propertyDto.Title ?? property.Title;
        property.Description = propertyDto.Description ?? property.Description;
        property.ProvinceId = propertyDto.ProvinceId ?? property.ProvinceId;
        property.DistrictId = propertyDto.DistrictId ?? property.DistrictId;
        property.Municipality = propertyDto.Municipality ?? property.Municipality;
        property.WardNumber = propertyDto.WardNumber ?? property.WardNumber;
        property.LandMark = propertyDto.LandMark ?? property.LandMark;
        property.PropertyType = propertyDto.PropertyType ?? property.PropertyType;
        property.IsAvailable = propertyDto.IsAvailable ?? property.IsAvailable;
        property.Price = propertyDto.Price ?? property.Price;
        property.EstimatedCommission = await _commissionService.CalculateCommissionAsync(property.Price);

        // update image if provided
        if (propertyDto.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(property.ImageUrl))
            {
                _fileService.RemoveFile(property.ImageUrl);
            }
            property.ImageUrl = await _fileService.SaveFileAsync(propertyDto.ImageFile);
        }

        await _unitOfWork.PropertyRepository.SaveChangesAsync();

        // Invalidate caches
        await _cacheService.RemoveAsync(CacheKeys.BrokerProperties(userId));
        await _cacheService.IncrementVersionAsync(CacheKeys.PropertiesVersionKey);
    }

    public async Task DeleteProperty(long id, long userId)
    {
        var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
        if (property == null) throw new NotFoundException("Property not found");
        if (property.BrokerId != userId) throw new UnauthorizedAccessException("You are not authorized to delete this property");

        if (!string.IsNullOrEmpty(property.ImageUrl))
        {
            _fileService.RemoveFile(property.ImageUrl);
        }

        _unitOfWork.PropertyRepository.Delete(property);
        await _unitOfWork.PropertyRepository.SaveChangesAsync();
        
        // Invalidate caches
        await _cacheService.RemoveAsync(CacheKeys.BrokerProperties(userId));
        await _cacheService.IncrementVersionAsync(CacheKeys.PropertiesVersionKey);
    }
}