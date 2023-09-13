using InvoiceAPI.Features.Services.ListByInvoiceId;
using InvoiceAPI.Models.Api;
using MediatR;
using Microservice.Common.Controllers;
using Microservice.Common.Features;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers
{
    public class InvoiceController : CRUDController<InvoiceDto>
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/Services")]
        public virtual async Task<IEnumerable<ServiceDto>> ServicesByInvoiceId(Guid id)
        {
            var result = await _mediator.Send(new ListServicesByInvoiceIdQuery(id));
            return result ?? Enumerable.Empty<ServiceDto>();
        }
    }
}
