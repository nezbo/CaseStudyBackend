using AutoMapper;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Presentation.Controllers;

namespace InvoiceAPI.Presentation.Controllers
{
    public class ServiceController(IMediator mediator, IMapper mapper)
                : CRUDController<ServiceDto, Service>(mediator, mapper)
    {
    }
}
