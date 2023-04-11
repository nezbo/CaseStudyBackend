namespace Microservice.Common.Repository;

public interface IGenericRepository<T>
{
    Task<T> GetByIdAsync(Guid id);
    IAsyncEnumerable<T> GetAll();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
