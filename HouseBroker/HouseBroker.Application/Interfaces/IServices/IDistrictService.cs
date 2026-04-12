using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IDistrictService
{
    Task<List<DistrictDto>> GetAllAsync();
    Task<DistrictDto?> GetByIdAsync(long id);
    Task<List<DistrictDto>> GetByProvinceIdAsync(long provinceId);
    Task CreateAsync(UpsertDistrictDto districtDto, long userId);
    Task UpdateAsync(long id, UpsertDistrictDto districtDto, long userId);
    Task DeleteAsync(long id);
}
