using Microservice.Common.Application.Repository;

namespace AssetAPI.Application.Repository;

public interface IUnitOfWork : IGenericUnitOfWork
{
    IAssetRepository AssetRepository { get; }
}
