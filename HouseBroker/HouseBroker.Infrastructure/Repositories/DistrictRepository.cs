using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;

namespace HouseBroker.Infrastructure.Repositories;

public class DistrictRepository(HouseBrokerDbContext context) : BaseRepository<District>(context), IDistrictRepository
{
    
}