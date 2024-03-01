using AssetAPI.Application.Repository;
using Microservice.Common.Infrastructure.Repository;

namespace AssetAPI.Infrastructure.Persistence.Repository;

public class UnitOfWork : GenericUnitOfWork, IUnitOfWork
{
    private readonly IServiceProvider _services;

    public UnitOfWork(IServiceProvider services, ApiDbContext context) : base(services, context)
    {
        _services = services;
    }

    public IAssetRepository AssetRepository => _services.GetService<IAssetRepository>()!;
}
