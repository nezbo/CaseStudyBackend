using AssetAPI.Application.Features.Assets;
using AssetAPI.Domain.Models;
using Microservice.Common.Test;
using Xunit;

namespace AssetAPI.Test.Features;

public class AssetCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<AssetCRUDCommandsHandler, Asset>
{
    protected override Asset InstantiateEntity(Guid id)
    {
        var result = new Asset(id)
        {
            Name = $"Asset {id}"
        };
        result.SetPrice(13.37M);

        return result;
    }
}
