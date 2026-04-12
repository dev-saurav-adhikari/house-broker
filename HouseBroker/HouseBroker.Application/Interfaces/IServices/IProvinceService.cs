using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IProvinceService
{
    Task<List<ProvinceDto>> GetAllAsync();
    Task<ProvinceDto?> GetByIdAsync(long id);
    Task CreateAsync(UpsertProvinceDto provinceDto, long userId);
    Task UpdateAsync(long id, UpsertProvinceDto provinceDto, long userId);
    Task DeleteAsync(long id);
}
