using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;

namespace HouseBroker.Infrastructure.Repositories;

public class CommissionRepository(HouseBrokerDbContext context)
    : BaseRepository<CommissionSetting>(context), ICommissionRepository
{
    
}