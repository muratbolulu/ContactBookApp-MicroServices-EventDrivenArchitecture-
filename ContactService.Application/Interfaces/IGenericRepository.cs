using System.Linq.Expressions;

namespace ContactService.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate = null,
                          Func<IQueryable<T>, IQueryable<T>> include = null);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate,
                                         Func<IQueryable<T>, IQueryable<T>> include = null);
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task SaveChangesAsync();
}
