using HouseBroker.Application.DTOs;
using HouseBroker.Infrastructure.Services;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IPropertyService
{
    Task<Pagination<PropertyDto>> GetAllProperties(PropertyFilterDto filter);
    Task InsertProperty(InsertPropertyDto property, long userId);
}