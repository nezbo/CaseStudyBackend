using ErrorOr;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Repository;

public interface IGenericRepository<T> where T : Entity
{
    Task<ErrorOr<T>> GetByIdAsync(Guid id);
    Task<ErrorOr<IEnumerable<T>>> GetByIdsAsync(params Guid[] ids);
    Task<ErrorOr<IEnumerable<T>>> GetAllAsync();
    Task<ErrorOr<Created>> AddAsync(T entity);
    Task<ErrorOr<Updated>> UpdateAsync(T entity);
    Task<ErrorOr<Deleted>> DeleteAsync(Guid id);
}
