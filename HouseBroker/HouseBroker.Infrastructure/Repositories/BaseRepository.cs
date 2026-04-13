using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HouseBroker.Application.Interfaces.IRepositories;
using HouseBroker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly HouseBrokerDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(HouseBrokerDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAll(bool asNoTracking = false)
    {
        return asNoTracking ? await _dbSet.AsNoTracking().ToListAsync() : await _dbSet.ToListAsync();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool asNoTracking = false)
    {
        return asNoTracking ? _dbSet.AsNoTracking().Where(condition) : _dbSet.Where(condition);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}