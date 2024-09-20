using ErrorOr;
using InvoiceAPI.Application.Features.Invoices.CreateByAssets;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Mapping;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Presentation.Controllers;
using Microservice.Common.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Presentation.Controllers
{
    public class InvoiceController(IMediator mediator)
                : CRUDController<InvoiceDto, Invoice>(mediator)
    {
        private readonly IMediator _mediator = mediator;

        protected override InvoiceDto MapFromDomain(Invoice model) => model.ToDto();
        protected override ErrorOr<Invoice> MapToDomain(InvoiceDto model) => model.ToDomain();

        [HttpPost("ByAssets")]
        public virtual async Task<IActionResult> CreateByAssets([FromBody] InvoiceDto invoice, [FromQuery(Name = "ids")] IEnumerable<Guid> assetIds)
        {
            var domainModel = this.MapToDomain(invoice);

            if (domainModel.IsError)
                return this.Problem(domainModel.Errors);

            var result = await _mediator.Send(new CreateByAssetsCommand { Data = domainModel.Value, AssetIds = assetIds });
            return this.MatchOrProblem(result, o => Ok(o));
        }
    }
}
