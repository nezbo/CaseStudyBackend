using AssetAPI.Persistence.Repositories;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence;

public class UnitOfWork : GenericUnitOfWork, IUnitOfWork
{
    private readonly IServiceProvider _services;

    public UnitOfWork(IServiceProvider services, ApiDbContext context) : base(services, context)
    {
        _services = services;
    }

    public IAssetRepository AssetRepository => _services.GetService<IAssetRepository>();
}
