using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;

namespace HouseBroker.Infrastructure.Repositories;

public class ProvinceRepository(HouseBrokerDbContext context) : BaseRepository<Province>(context), IProvinceRepository
{
    
}