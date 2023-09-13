using InvoiceAPI.Models.Database;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence.Repositories;

public interface IServiceRepository : IGenericRepository<Service>
{
    Task<IEnumerable<Service>> GetByInvoiceAsync(Guid invoiceId);
}
