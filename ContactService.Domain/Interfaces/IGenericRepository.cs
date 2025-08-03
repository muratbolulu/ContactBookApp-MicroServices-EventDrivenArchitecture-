using System.Linq.Expressions;

namespace ContactService.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    //Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    //Task DeleteByIdAsync(Guid id);
    Task SaveChangesAsync();
}
