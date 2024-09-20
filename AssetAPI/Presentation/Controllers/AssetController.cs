using AssetAPI.Application.Features.Assets.ListAssetsValidOn;
using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Mapping;
using AssetAPI.Presentation.Models;
using ErrorOr;
using MediatR;
using Microservice.Common.Extensions;
using Microservice.Common.Presentation.Controllers;
using Microservice.Common.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AssetAPI.Controllers
{
    public class AssetController(IMediator mediator) 
        : CRUDController<AssetDto, Asset>(mediator)
    {
        private readonly IMediator _mediator = mediator;

        protected override AssetDto MapFromDomain(Asset model) => model.ToDto();

        protected override ErrorOr<Asset> MapToDomain(AssetDto model) => model.ToDomain();

        [HttpGet("ValidOn")]
        public async Task<ActionResult> List([FromQuery]DateOnly date)
        {
            var response = await _mediator.Send(new ListAssetsValidOnQuery(date));
            return this.MatchOrProblem(response, l => Ok(l.ForEachThen(v => this.SetEditUrl(v.ToDto()))));
        }
    }
}
