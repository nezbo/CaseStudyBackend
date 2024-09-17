using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Repository;

public interface IGenericRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetByIdsAsync(params Guid[] ids);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
