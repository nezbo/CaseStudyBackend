using AutoMapper;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Features.Invoices;

public class InvoiceCRUDCommandsHandler : BasicCRUDCommandsHandler<InvoiceDto, Invoice>
{
    public InvoiceCRUDCommandsHandler(IMediator mediator, IGenericUnitOfWork unitOfWork, IMapper mapper)
        : base(mediator, unitOfWork, mapper)
    {
    }
}
