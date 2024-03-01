using AutoMapper;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace InvoiceAPI.Application.Features.Services;

public class ServiceCRUDCommandsHandler : BasicCRUDCommandsHandler<ServiceDto, Service>
{
    public ServiceCRUDCommandsHandler(IMediator mediator, IGenericUnitOfWork unitOfWork, IMapper mapper)
        : base(mediator, unitOfWork, mapper)
    {
    }
}
