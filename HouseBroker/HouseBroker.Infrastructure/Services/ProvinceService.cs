using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class ProvinceService(IUnitOfWork _unitOfWork) : IProvinceService
{
    public async Task<List<ProvinceDto>> GetAllAsync()
    {
        var provinces = await _unitOfWork.ProvinceRepository.GetAll(asNoTracking: true);
        return provinces.Select(p => new ProvinceDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }

    public async Task<ProvinceDto?> GetByIdAsync(long id)
    {
        var province = await _unitOfWork.ProvinceRepository.GetByIdAsync(id);
        if (province == null) return null;

        return new ProvinceDto
        {
            Id = province.Id,
            Name = province.Name
        };
    }

    public async Task CreateAsync(UpsertProvinceDto provinceDto, long userId)
    {
        var province = new Province
        {
            Name = provinceDto.Name,
            CreatedBy = userId
        };

        await _unitOfWork.ProvinceRepository.AddAsync(province);
        await _unitOfWork.ProvinceRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, UpsertProvinceDto provinceDto, long userId)
    {
        var province = await _unitOfWork.ProvinceRepository.GetByIdAsync(id);
        if (province == null) return;

        province.Name = provinceDto.Name;
        province.ModifiedBy = userId;
        province.ModifiedOn = DateTimeOffset.UtcNow;

        _unitOfWork.ProvinceRepository.Update(province);
        await _unitOfWork.ProvinceRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var province = await _unitOfWork.ProvinceRepository.GetByIdAsync(id);
        if (province == null) return;

        _unitOfWork.ProvinceRepository.Delete(province);
        await _unitOfWork.ProvinceRepository.SaveChangesAsync();
    }
}
