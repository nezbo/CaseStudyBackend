using AssetAPI.Persistence.Repositories;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence;

public interface IUnitOfWork : IGenericUnitOfWork
{
    IAssetRepository AssetRepository { get; }
}
