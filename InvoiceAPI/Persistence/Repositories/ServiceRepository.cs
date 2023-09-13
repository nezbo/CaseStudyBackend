using InvoiceAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Persistence.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Service>> GetByInvoiceIdAsync(Guid invoiceId)
    {
        return await DbSet
            .Where(s => s.InvoiceId == invoiceId)
            .ToListAsync();
    }
}
