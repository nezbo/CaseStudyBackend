using InvoiceAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Features.Invoices;

public class InvoiceCRUDCommandsHandler(IMediator mediator, IGenericRepository<Invoice> unitOfWork) 
    : BasicCRUDCommandsHandler<Invoice>(mediator, unitOfWork)
{
}
