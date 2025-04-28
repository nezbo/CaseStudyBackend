using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

public partial class ServiceRepository : IServiceRepository
{
    public Task<IEnumerable<Service>> GetByAssetIdAsync(Guid assetId)
    {
        return Task.FromResult(DbSet.Where(s => s.AssetId == assetId)
            .AsEnumerable());
    }
}
