using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

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
