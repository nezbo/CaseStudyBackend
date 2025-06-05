using InvoiceAPI.Domain.Models;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Repository;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    public Task<IEnumerable<Invoice>> GetByAssetIdAsync(Guid assetId);
}
