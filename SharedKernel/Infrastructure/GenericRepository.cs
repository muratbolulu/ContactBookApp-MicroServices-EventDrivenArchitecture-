﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.Interface;
using System.Linq.Expressions;

namespace SharedKernel.Infrastructure;

public class GenericRepository<T, TContext> : IGenericRepository<T>
    where T : class
    where TContext : DbContext
{
    protected readonly TContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate,
                                                      Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (include != null)
        {
            query = include(query);
        }

        return await query.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task DeleteAsync(T entity) => _dbSet.Remove(entity);

    public async Task UpdateAsync(T entity) => _dbSet.Update(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

}
