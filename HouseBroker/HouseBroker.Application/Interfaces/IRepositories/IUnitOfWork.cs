namespace HouseBroker.Application.Interfaces.IRepositories;

public interface IUnitOfWork
{
  Task  BeginTransactionAsync();
  Task SaveAsync();
  
  IPropertyRepository PropertyRepository { get; }
  ICommissionRepository CommissionRepository { get; }
  IDistrictRepository DistrictRepository { get; }
  IProvinceRepository ProvinceRepository { get; }
}