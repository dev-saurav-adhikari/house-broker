using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace HouseBroker.Infrastructure.Repositories;

public class UnitOfWork (IServiceProvider _serviceProvider, HouseBrokerDbContext _dbContext) : IUnitOfWork
{
    public async Task BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task SaveAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CommitTransactionAsync();
            }

        }
        catch (Exception)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
            throw;
        }
    }

    public IPropertyRepository PropertyRepository => _serviceProvider.GetRequiredService<IPropertyRepository>();
    public ICommissionRepository CommissionRepository => _serviceProvider.GetRequiredService<ICommissionRepository>();
    public IDistrictRepository DistrictRepository => _serviceProvider.GetRequiredService<IDistrictRepository>();
    public IProvinceRepository ProvinceRepository => _serviceProvider.GetRequiredService<IProvinceRepository>();
}