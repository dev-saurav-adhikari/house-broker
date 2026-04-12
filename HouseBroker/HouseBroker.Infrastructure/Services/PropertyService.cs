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
    public async Task<Pagination<PropertyDto>> GetAllProperties(PropertyFilterDto filter)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request.Host}";
        
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

        var query =  (from p in allProperties
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
                EstimatedCommission = p.EstimatedCommission,
            });//.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        return new Pagination<PropertyDto>(query, filter.PageNumber, filter.PageSize);
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
            EstimatedCommission = await CommissionCalculation(property.Price)
        };
        await _unitOfWork.PropertyRepository.AddAsync(newProperty);
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