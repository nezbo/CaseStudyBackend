using AutoMapper;
using InvoiceAPI.Application.Features.Invoices.CreateByAssets;
using InvoiceAPI.Application.Features.Services.ListByInvoiceId;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace InvoiceAPI.Presentation.Controllers
{
    public class InvoiceController(IMediator mediator, IMapper mapper)
                : CRUDController<InvoiceDto, Invoice>(mediator, mapper)
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}/Services")]
        public virtual async Task<IEnumerable<ServiceDto>> ServicesByInvoiceId(Guid id)
        {
            var result = await _mediator.Send(new ListServicesByInvoiceIdQuery(id));
            return (result ?? [])
                .ForEach(TrySetEditUrl);
        }

        [HttpPost("ByAssets")]
        public virtual async Task<IActionResult> CreateByAssets([FromBody] Invoice invoice, [FromQuery] IEnumerable<Guid> assetIds)
        {
            var result = await _mediator.Send(new CreateByAssetsCommand { Data = invoice, AssetIds = assetIds });
            return result.Match(o => Ok(o), e => Problem());
        }
    }
}
