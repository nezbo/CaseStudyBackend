using AssetAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace AssetAPI.Application.Features.Assets;

public class AssetCRUDCommandsHandler(IMediator mediator, IGenericRepository<Asset> repository) 
    : BasicCRUDCommandsHandler<Asset>(mediator, repository)
{
}
