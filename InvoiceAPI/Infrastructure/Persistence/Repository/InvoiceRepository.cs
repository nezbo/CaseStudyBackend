using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace InvoiceAPI.Infrastructure.Persistence.Repository;

public class InvoiceRepository(IBaseDbContext dbContext) 
    : GenericRepository<Invoice>(dbContext), IInvoiceRepository
{
}
