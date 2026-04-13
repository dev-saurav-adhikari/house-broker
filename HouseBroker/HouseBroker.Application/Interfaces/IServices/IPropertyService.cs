using HouseBroker.Application.DTOs;
using HouseBroker.Infrastructure.Services;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IPropertyService
{
    Task<Pagination<PropertyDetailWithBrokerInfoDto>> GetAllPropertiesAsync(PropertyFilterDto filter);
    Task InsertProperty(InsertPropertyDetailDto propertyDetail, long userId);
    Task UpdateProperty(long id, UpdatePropertyDto property, long userId);
    Task DeleteProperty(long id, long userId);
}