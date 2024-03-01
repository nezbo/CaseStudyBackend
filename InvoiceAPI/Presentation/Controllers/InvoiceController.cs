using InvoiceAPI.Application.Features.Invoices.CreateByAssets;
using InvoiceAPI.Application.Features.Services.ListByInvoiceId;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Presentation.Controllers
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
            return (result ?? Enumerable.Empty<ServiceDto>())
                .ForEach(TrySetEditUrl);
        }

        [HttpPost("ByAssets")]
        public virtual async Task<InvoiceDto?> CreateByAssets([FromBody] InvoiceDto invoice, [FromQuery] IEnumerable<Guid> assetIds)
        {
            var result = await _mediator.Send(new CreateByAssetsCommand { Data = invoice, AssetIds = assetIds });
            return await Read(result);
        }
    }
}
