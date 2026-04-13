using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class DistrictService(IUnitOfWork _unitOfWork) : IDistrictService
{
    public async Task<List<DistrictDto>> GetAllAsync()
    {
        var districts = await _unitOfWork.DistrictRepository
            .FindByCondition(_ => true, asNoTracking: true)
            .Include(d => d.Province)
            .Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,
                ProvinceId = d.ProvinceId,
                ProvinceName = d.Province.Name
            }).ToListAsync();
            
        return districts;
    }

    public async Task<DistrictDto?> GetByIdAsync(long id)
    {
        var district = await _unitOfWork.DistrictRepository
            .FindByCondition(d => d.Id == id)
            .Include(d => d.Province)
            .FirstOrDefaultAsync();
            
        if (district == null) return null;

        return new DistrictDto
        {
            Id = district.Id,
            Name = district.Name,
            ProvinceId = district.ProvinceId,
            ProvinceName = district.Province.Name
        };
    }

    public async Task<List<DistrictDto>> GetByProvinceIdAsync(long provinceId)
    {
        var districts = await _unitOfWork.DistrictRepository
            .FindByCondition(d => d.ProvinceId == provinceId, asNoTracking: true)
            .Include(d => d.Province)
            .Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,
                ProvinceId = d.ProvinceId,
                ProvinceName = d.Province.Name
            }).ToListAsync();
            
        return districts;
    }

    public async Task CreateAsync(UpsertDistrictDto districtDto, long userId)
    {
        var district = new District
        {
            Name = districtDto.Name,
            ProvinceId = districtDto.ProvinceId,
            CreatedBy = userId
        };

        await _unitOfWork.DistrictRepository.AddAsync(district);
        await _unitOfWork.DistrictRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, UpsertDistrictDto districtDto, long userId)
    {
        var district = await _unitOfWork.DistrictRepository.GetByIdAsync(id);
        if (district == null) return;

        district.Name = districtDto.Name;
        district.ProvinceId = districtDto.ProvinceId;
        district.ModifiedBy = userId;
        district.ModifiedOn = DateTimeOffset.UtcNow;

        _unitOfWork.DistrictRepository.Update(district);
        await _unitOfWork.DistrictRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var district = await _unitOfWork.DistrictRepository.GetByIdAsync(id);
        if (district == null) return;

        _unitOfWork.DistrictRepository.Delete(district);
        await _unitOfWork.DistrictRepository.SaveChangesAsync();
    }
}
