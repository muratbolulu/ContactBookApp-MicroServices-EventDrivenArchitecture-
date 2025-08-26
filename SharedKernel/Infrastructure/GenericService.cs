using SharedKernel.Interface;
using System.Linq.Expressions;

namespace SharedKernel.Infrastructure;

public class GenericService<T> : IGenericService<T>
    where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericService(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate = null, 
                                 Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        return await _repository.GetByIdAsync(predicate, include);
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.GetWhereAsync(predicate);
    }
    public async Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, 
                                                      Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        return await _repository.GetWhereAsync(predicate, include);
    }

    public async Task AddAsync(T entity) => await _repository.AddAsync(entity);

    public async Task DeleteAsync(T entity) => await _repository.DeleteAsync(entity);

    public async Task UpdateAsync(T entity) => await _repository.UpdateAsync(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.AnyAsync(predicate);
    }

    public async Task SaveChangesAsync() => await _repository.SaveChangesAsync();

}
