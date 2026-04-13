using HouseBroker.Application.CustomException;
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
    IFileService _fileService) : IPropertyService


{
    public Pagination<PropertyDetailWithBrokerInfoDto> GetAllProperties(PropertyFilterDto filter)
    {
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
        
        // select distinct broker id from available properties
        var brokerIds = allProperties.Select(p => p.BrokerId).Distinct();

        // fetch detail information of broker
        var brokers = _userManager.Users
            .Where(u => brokerIds.Contains(u.Id))
            .Select(p => new { p.Id, p.Email, p.PhoneNumber });

        // query build
        var query =  (from p in allProperties
            join r in brokers on p.BrokerId equals r.Id
            select new PropertyDetailWithBrokerInfoDto
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
                BrokerName = r.Email,
                EstimatedCommission = p.EstimatedCommission,
                BrokerEmail = r.Email,
                BrokerPhone = r.PhoneNumber,
            });
        // query execution and pagination
        return new Pagination<PropertyDetailWithBrokerInfoDto>(query, filter.PageNumber, filter.PageSize);
    }

    public async Task InsertProperty(InsertPropertyDetailDto propertyDetail, long userId)
    {
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
            EstimatedCommission = await CommissionCalculation(propertyDetail.Price) // commission calculation
        };
        // adding new property
        await _unitOfWork.PropertyRepository.AddAsync(newProperty);
        await _unitOfWork.PropertyRepository.SaveChangesAsync();
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
        property.EstimatedCommission = await CommissionCalculation(property.Price);

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
    }

    private async Task<decimal> CommissionCalculation(decimal price)
    {
        if (price <= 0) return 0;

        var tiers = await _unitOfWork.CommissionRepository.GetAll();

        // find the tier where price is greater than the min and less or equal to the max
        var matchingTier = tiers.FirstOrDefault(t =>
            (price > t.MinimumAmount || (t.MinimumAmount == 0 && price >= 0)) &&
            (t.MaximumAmount <= 0 || price <= (decimal)t.MaximumAmount));

        if (matchingTier == null) return 0;

        var commission = price * (matchingTier.Rate / 100);
        return Math.Round(commission, 2, MidpointRounding.AwayFromZero);
    }
}