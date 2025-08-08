using System.Linq.Expressions;

namespace SharedKernel.Interface;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate); 
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
    Task SaveChangesAsync();
}
