using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }
}
