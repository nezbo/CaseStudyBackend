using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Repository;

public interface IGenericUnitOfWork
{
    Task<int> CommitAsync();
    IGenericRepository<T>? GetRepository<T>() where T : class, IIdentity;
}
