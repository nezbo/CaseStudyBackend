using InvoiceAPI.Persistence.Repositories;
using Microservice.Common.Repository;

namespace InvoiceAPI.Persistence;

public class UnitOfWork : GenericUnitOfWork, IUnitOfWork
{
    private readonly IServiceProvider _services;

    public UnitOfWork(IServiceProvider services, InvoiceDbContext context) : base(services, context)
    {
        _services = services;
    }

    public IInvoiceRepository InvoiceRepository => _services.GetService<IInvoiceRepository>()!;

    public IServiceRepository ServiceRepository => _services.GetService<IServiceRepository>()!;
}
