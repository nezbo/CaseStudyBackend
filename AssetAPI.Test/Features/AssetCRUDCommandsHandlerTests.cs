using AssetAPI.Features.Assets;
using AssetAPI.Models.Api;
using AssetAPI.Models.Database;
using Microservice.Common.Test;

namespace AssetAPI.Test.Features;

public class AssetCRUDCommandsHandlerTests : BasicCRUDCommandsHandlerTests<AssetCRUDCommandsHandler, AssetDto, Asset>
{
    protected override AssetDto InstantiateApiEntity(Guid id)
    {
        return new AssetDto
        {
            Id = id,
            Name = $"Asset {id}",
            Price = 13.37M,
        };
    }

    protected override Asset InstantiateDbEntity(Guid id)
    {
        return new Asset
        {
            Id = id,
            Name = $"Asset {id}",
            Price = 13.37M,
        };
    }
}
