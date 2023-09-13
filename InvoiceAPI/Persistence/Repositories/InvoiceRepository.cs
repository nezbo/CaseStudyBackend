using InvoiceAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }
}
