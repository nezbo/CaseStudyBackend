using Microservice.Common.Models;

namespace Microservice.Common.Repository;

public interface IGenericRepository<T> where T : class, IIdentity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetByIdsAsync(params Guid[] ids);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
