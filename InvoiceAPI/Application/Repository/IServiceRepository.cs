using InvoiceAPI.Domain.Models;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Repository;

public interface IServiceRepository : IGenericRepository<Service>
{
    Task<IEnumerable<Service>> GetByInvoiceIdAsync(Guid invoiceId);
}
