using Microservice.Common.Models;

namespace Microservice.Common.Repository;

public interface IGenericRepository<T> where T : class, IIdentity
{
    Task<T> GetByIdAsync(Guid id);
    IAsyncEnumerable<T> GetAll();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
