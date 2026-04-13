using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Interfaces.IServices;

public interface ICommissionService
{
    Task<List<CommissionDto>> GetAllAsync();
    Task<CommissionDto?> GetByIdAsync(long id);
    Task CreateAsync(UpsertCommissionDto dto, long userId);
    Task UpdateAsync(long id, UpsertCommissionDto dto, long userId);
    Task DeleteAsync(long id);
    
    /// <summary>
    /// Calculates commission for a given price based on commission settings.
    /// </summary>
    Task<decimal> CalculateCommissionAsync(decimal price);
}
