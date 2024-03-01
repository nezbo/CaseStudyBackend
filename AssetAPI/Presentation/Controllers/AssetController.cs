using AssetAPI.Application.Features.Assets.ListAssetsValidOn;
using AssetAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Presentation.Controllers;
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

        [HttpGet("ValidOn")]
        public async Task<IEnumerable<AssetDto>> List([FromQuery]DateOnly date)
        {
            return (await _mediator.Send(new ListAssetsValidOnQuery(date)))
                .ForEach(TrySetEditUrl);
        }
    }
}
