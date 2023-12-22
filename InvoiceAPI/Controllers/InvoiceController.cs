using InvoiceAPI.Features.Invoices.CreateByAssets;
using InvoiceAPI.Features.Services.ListByInvoiceId;
using InvoiceAPI.Models.Api;
using MediatR;
using Microservice.Common.Controllers;
using Microservice.Common.Extensions;
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
            return (result ?? Enumerable.Empty<ServiceDto>())
                .ForEach(TrySetEditUrl);
        }

        [HttpPost("ByAssets")]
        public virtual async Task<InvoiceDto?> CreateByAssets([FromBody] InvoiceDto invoice, [FromQuery] IEnumerable<Guid> assetIds)
        {
            var result = await _mediator.Send(new CreateByAssetsCommand { Data = invoice, AssetIds = assetIds});
            return await Read(result);
        }
    }
}
