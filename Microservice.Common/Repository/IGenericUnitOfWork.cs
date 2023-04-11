namespace Microservice.Common.Repository;

public interface IGenericUnitOfWork
{
    Task<int> CommitAsync();
    IGenericRepository<T> GetRepository<T>();
}
