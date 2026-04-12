using System.Linq.Expressions;
using HouseBroker.Application.Common;

namespace HouseBroker.Application.Interfaces.IRepositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id);

    Task<List<T>> GetAll(bool asNoTracking = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool asNoTracking = false);

    Task<T> AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task SaveChangesAsync();
}