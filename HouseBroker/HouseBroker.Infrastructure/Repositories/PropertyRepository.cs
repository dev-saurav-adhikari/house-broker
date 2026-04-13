using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Persistence;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyRepository(HouseBrokerDbContext context) : BaseRepository<Property>(context), IPropertyRepository
{
    
}