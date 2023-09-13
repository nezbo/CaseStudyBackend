using InvoiceAPI.Models.Database;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence.Repositories;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
}
