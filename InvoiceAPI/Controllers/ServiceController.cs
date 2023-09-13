using InvoiceAPI.Models.Api;
using MediatR;
using Microservice.Common.Controllers;

namespace InvoiceAPI.Controllers
{
    public class ServiceController : CRUDController<ServiceDto>
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }
    }
}
