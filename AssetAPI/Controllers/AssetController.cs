using AssetAPI.Features.Assets.ListAssetsValidOn;
using AssetAPI.Models.Api;
using MediatR;
using Microservice.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AssetAPI.Controllers
{
    public class AssetController : CRUDController<AssetDto>
    {
        private readonly IMediator _mediator;

        public AssetController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [NonAction]
        public override Task<IEnumerable<AssetDto>> List() { return null; }

        [HttpGet]
        public async Task<IEnumerable<AssetDto>> List([FromQuery]DateOnly? validOn)
        {
            if (!validOn.HasValue)
                return await base.List();

            return await _mediator.Send(new ListAssetsValidOnQuery(validOn.Value));
        }
    }
}
