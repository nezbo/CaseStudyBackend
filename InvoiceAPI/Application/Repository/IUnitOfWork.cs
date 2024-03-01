using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Repository;

public interface IUnitOfWork : IGenericUnitOfWork
{
    IInvoiceRepository InvoiceRepository { get; }
    IServiceRepository ServiceRepository { get; }
}
