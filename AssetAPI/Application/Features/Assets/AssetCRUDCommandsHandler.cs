using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Models;
using AutoMapper;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace AssetAPI.Application.Features.Assets;

public class AssetCRUDCommandsHandler : BasicCRUDCommandsHandler<AssetDto, Asset>
{
    public AssetCRUDCommandsHandler(IMediator mediator, IGenericUnitOfWork unitOfWork, IMapper mapper)
        : base(mediator, unitOfWork, mapper)
    {
    }
}
