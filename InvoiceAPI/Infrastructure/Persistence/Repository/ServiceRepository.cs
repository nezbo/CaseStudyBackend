using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

public class ServiceRepository(IBaseDbContext dbContext)
    : GenericRepository<Service>(dbContext), IServiceRepository
{
    public Task<IEnumerable<Service>> GetByAssetIdAsync(Guid assetId)
    {
        return Task.FromResult(DbSet.Where(s => s.AssetId == assetId)
            .AsEnumerable());
    }
}
