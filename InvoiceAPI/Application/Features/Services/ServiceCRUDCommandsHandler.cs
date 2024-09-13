using InvoiceAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Features.Services;

public class ServiceCRUDCommandsHandler(IMediator mediator, IGenericRepository<Service> unitOfWork) 
    : BasicCRUDCommandsHandler<Service>(mediator, unitOfWork)
{
}
