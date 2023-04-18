using AssetAPI.Models.Database;
using AssetAPI.Persistence.Repositories;
using Microservice.Common.Test;

namespace AssetAPI.Test.Persistence.Repositories;

public class AssetRepositoryTests : GenericRepositoryTests<AssetRepository, Asset>
{
    protected override Asset InstantiateEntity(Guid id)
    {
        return new Asset();
    }
}
