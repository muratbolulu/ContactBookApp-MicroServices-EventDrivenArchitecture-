using Microsoft.EntityFrameworkCore;
using ReportService.Application.Interfaces;
using System.Linq.Expressions;

namespace ReportService.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly ReportDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ReportDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
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
