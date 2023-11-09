using InvoiceAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<Service>> GetByInvoiceIdAsync(Guid invoiceId)
    {
        var query = () => DbSet
            .Where(s => s.InvoiceId == invoiceId)
            .ToList()
            .AsEnumerable();

        return Task.Run(query);
    }
}
