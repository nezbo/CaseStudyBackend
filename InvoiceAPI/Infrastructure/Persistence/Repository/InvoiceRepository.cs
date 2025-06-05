using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

public class InvoiceRepository(InvoiceDbContext dbContext)
    : GenericRepository<Invoice>(dbContext), IInvoiceRepository
{
    public Task<IEnumerable<Invoice>> GetByAssetIdAsync(Guid assetId)
    {
        return this.DbSet.Where(i => i._services
            .Any(s => s.AssetId == assetId))
            .ToListAsync()
            .ContinueWith(t => t.Result.AsEnumerable());
    }
}
