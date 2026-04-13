using HouseBroker.Application.Common;
using HouseBroker.Application.CustomException;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class BrokerService(
    UserManager<IdentityUser<long>> _userManager,
    RoleManager<IdentityRole<long>> _roleManager,
    IUnitOfWork _unitOfWork)
    : IBrokerService
{
    public async Task<Pagination<BrokerDto>> GetBrokersAsync(BasicFilterDto filter)
    {
        var brokerRole = await _roleManager.FindByNameAsync("BROKER");
        if (brokerRole == null)
        {
            return new Pagination<BrokerDto>(new List<BrokerDto>(), 0, filter.PageNumber, filter.PageSize);
        }

        var query = _userManager.Users.Where(p =>
            (string.IsNullOrWhiteSpace(filter.Search)
             || p.Email!.Contains(filter.Search)
             || p.UserName!.Contains(filter.Search))).Select(s => new BrokerDto
        {
            Id = s.Id,
            Email = s.Email!,
            PhoneNumber = s.PhoneNumber,
            UserName = s.UserName!,
        });

        return new Pagination<BrokerDto>(query, filter.PageNumber, filter.PageSize);
    }

    public async Task<BrokerDto?> GetBrokerByIdAsync(long id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        var isBroker = await _userManager.IsInRoleAsync(user, "BROKER");
        if (!isBroker) return null;

        return new BrokerDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName ?? string.Empty,
        };
    }

    public async Task<BrokerPropertiesDto> GetPropertiesByBrokerIdAsync(long brokerId,
        BasicFilterDto filter)
    {
        var broker = (await _userManager.FindByIdAsync(brokerId.ToString())) ??
                     throw new BadRequestException("Invalid broker");

        var query = _unitOfWork.PropertyRepository.FindByCondition(p => p.BrokerId == brokerId &&
                                                                        (string.IsNullOrWhiteSpace(filter.Search) ||
                                                                         p.Title.Contains(filter.Search) ||
                                                                         p.Description.Contains(filter.Search)));

        var projectedQuery = query.Select(p => new BrokerPropertyDetailDto
        {
            Id = p.Id,
            Description = p.Description,
            PropertyType = p.PropertyType,
            Price = p.Price,
            //ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? null : $"{baseUrl}{p.ImageUrl}",
            ProvinceId = p.ProvinceId,
            Municipality = p.Municipality,
            WardNumber = p.WardNumber,
            DistrictId = p.DistrictId,
            LandMark = p.LandMark,
            Title = p.Title,
            EstimatedCommission = p.EstimatedCommission
        });

        var properties =  new Pagination<BrokerPropertyDetailDto>(projectedQuery, filter.PageNumber, filter.PageSize);
        var brokerProperties =  new BrokerPropertiesDto
        {
            Properties = properties,
            PhoneNumber = broker.PhoneNumber!,
            Email = broker.Email!,
            UserName = broker.UserName!,
            Id = broker.Id
        };
        return brokerProperties;

    }

    public async Task<decimal> TotalEstimatedCommissionAsync(long brokerId)
    {
        return await _unitOfWork.PropertyRepository.FindByCondition(p => p.BrokerId == brokerId).SumAsync(x=>x.EstimatedCommission);
    }
}