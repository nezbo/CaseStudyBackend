using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Presentation.Controllers;

namespace InvoiceAPI.Presentation.Controllers
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
