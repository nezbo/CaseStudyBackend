using AutoMapper;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;
using MediatR;
using Microservice.Common.Features;
using Microservice.Common.Repository;

namespace InvoiceAPI.Features.Services;

public class ServiceCRUDCommandsHandler : BasicCRUDCommandsHandler<ServiceDto, Service>
{
    public ServiceCRUDCommandsHandler(IMediator mediator, IGenericUnitOfWork unitOfWork, IMapper mapper) 
        : base(mediator, unitOfWork, mapper)
    {
    }
}
