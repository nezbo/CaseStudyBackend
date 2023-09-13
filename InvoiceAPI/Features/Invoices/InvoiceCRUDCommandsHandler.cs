using AutoMapper;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;
using Microservice.Common.Features;
using Microservice.Common.Repository;

namespace InvoiceAPI.Features.Invoices;

public class InvoiceCRUDCommandsHandler : BasicCRUDCommandsHandler<InvoiceDto, Invoice>
{
    public InvoiceCRUDCommandsHandler(IGenericUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}
