using Microservice.Common.Models;

namespace Microservice.Common.Repository;

public interface IGenericUnitOfWork
{
    Task<int> CommitAsync();
    IGenericRepository<T>? GetRepository<T>() where T : class, IIdentity;
}
