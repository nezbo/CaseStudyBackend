using AssetAPI.Application.Features.Assets.ListAssetsValidOn;
using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Models;
using AutoMapper;
using MediatR;
using Microservice.Common.Extensions;
using Microservice.Common.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AssetAPI.Controllers
{
    public class AssetController(IMediator mediator, IMapper mapper) 
        : CRUDController<AssetDto, Asset>(mediator, mapper)
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("ValidOn")]
        public async Task<IEnumerable<AssetDto>> List([FromQuery]DateOnly date)
        {
            return (await _mediator.Send(new ListAssetsValidOnQuery(date)))
                .ForEachThen(SetEditUrl);
        }
    }
}
