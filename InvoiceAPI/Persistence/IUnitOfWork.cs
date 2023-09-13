using InvoiceAPI.Persistence.Repositories;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence;

public interface IUnitOfWork : IGenericUnitOfWork
{
    IInvoiceRepository InvoiceRepository { get; }
    IServiceRepository ServiceRepository { get; }
}
