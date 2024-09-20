using AssetAPI.Application.Features.Assets;
using AssetAPI.Domain.Models;
using Microservice.Common.Test;

namespace AssetAPI.Test.Features;

public class AssetCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<AssetCRUDCommandsHandler, Asset>
{
    protected override Asset InstantiateEntity(Guid id) 
        => Asset.Create(id, $"Asset {id}", 13.37M, null, null).Value;
}
