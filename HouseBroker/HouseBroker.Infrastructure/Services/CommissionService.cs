using HouseBroker.Application.Constants;
using HouseBroker.Application.CustomException;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;

namespace HouseBroker.Infrastructure.Services;

public class CommissionService(
    IUnitOfWork _unitOfWork,
    ICacheService _cacheService) : ICommissionService
{
    public async Task<List<CommissionDto>> GetAllAsync()
    {
        // check cache first
        var cached = await _cacheService.GetAsync<List<CommissionDto>>(CacheKeys.CommissionsKey);
        if (cached != null) return cached;

        var commissions = await _unitOfWork.CommissionRepository.GetAll(asNoTracking: true);
        var result = commissions.Select(c => new CommissionDto
        {
            Id = c.Id,
            MinimumAmount = c.MinimumAmount,
            MaximumAmount = c.MaximumAmount,
            Rate = c.Rate
        }).ToList();

        await _cacheService.SetAsync(CacheKeys.CommissionsKey, result, TimeSpan.FromDays(7));
        return result;
    }

    public async Task<CommissionDto?> GetByIdAsync(long id)
    {
        var commission = await _unitOfWork.CommissionRepository.GetByIdAsync(id);
        if (commission == null) return null;

        return new CommissionDto
        {
            Id = commission.Id,
            MinimumAmount = commission.MinimumAmount,
            MaximumAmount = commission.MaximumAmount,
            Rate = commission.Rate
        };
    }

    public async Task CreateAsync(UpsertCommissionDto dto, long userId)
    {
        var commission = new CommissionSetting
        {
            MinimumAmount = dto.MinimumAmount,
            MaximumAmount = dto.MaximumAmount,
            Rate = dto.Rate,
            CreatedBy = userId
        };

        await _unitOfWork.CommissionRepository.AddAsync(commission);
        await _unitOfWork.CommissionRepository.SaveChangesAsync();

        // invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.CommissionsKey);
    }

    public async Task UpdateAsync(long id, UpsertCommissionDto dto, long userId)
    {
        var commission = await _unitOfWork.CommissionRepository.GetByIdAsync(id);
        if (commission == null) throw new NotFoundException("Commission setting not found");

        commission.MinimumAmount = dto.MinimumAmount;
        commission.MaximumAmount = dto.MaximumAmount;
        commission.Rate = dto.Rate;
        commission.ModifiedBy = userId;
        commission.ModifiedOn = DateTimeOffset.UtcNow;

        _unitOfWork.CommissionRepository.Update(commission);
        await _unitOfWork.CommissionRepository.SaveChangesAsync();

        // invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.CommissionsKey);
    }

    public async Task DeleteAsync(long id)
    {
        var commission = await _unitOfWork.CommissionRepository.GetByIdAsync(id);
        if (commission == null) throw new NotFoundException("Commission setting not found");

        _unitOfWork.CommissionRepository.Delete(commission);
        await _unitOfWork.CommissionRepository.SaveChangesAsync();

        // invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.CommissionsKey);
    }

    public async Task<decimal> CalculateCommissionAsync(decimal price)
    {
        if (price <= 0) return 0;

        // fetch from cache or db
        var commissions = await _cacheService.GetAsync<List<CommissionSetting>>(CacheKeys.CommissionsKey);
        if (commissions == null)
        {
            commissions = await _unitOfWork.CommissionRepository.GetAll();
            await _cacheService.SetAsync(CacheKeys.CommissionsKey, commissions, TimeSpan.FromDays(7));
        }

        // find the tier where price is greater than the min and less or equal to the max
        var matchingTier = commissions.FirstOrDefault(t =>
            (price > t.MinimumAmount || (t.MinimumAmount == 0 && price >= 0)) &&
            (t.MaximumAmount <= 0 || price <= (decimal)t.MaximumAmount));

        if (matchingTier == null) return 0;

        var commission = price * (matchingTier.Rate / 100);
        return Math.Round(commission, 2, MidpointRounding.AwayFromZero);
    }
}
