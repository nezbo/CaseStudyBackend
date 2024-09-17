using AutoMapper;
using InvoiceAPI.Application.Features.Invoices.CreateByAssets;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Presentation.Controllers;
using Microservice.Common.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Presentation.Controllers
{
    public class InvoiceController(IMediator mediator, IMapper mapper)
                : CRUDController<InvoiceDto, Invoice>(mediator, mapper)
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("ByAssets")]
        public virtual async Task<IActionResult> CreateByAssets([FromBody] InvoiceDto invoice, [FromQuery] IEnumerable<Guid> assetIds)
        {
            var domainModel = _mapper.Map<Invoice>(invoice);
            var result = await _mediator.Send(new CreateByAssetsCommand { Data = domainModel, AssetIds = assetIds });
            return this.MatchOrProblem(result, o => Ok(o));
        }
    }
}
