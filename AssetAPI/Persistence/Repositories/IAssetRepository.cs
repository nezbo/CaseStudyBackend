using AssetAPI.Models.Database;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence.Repositories;

public interface IAssetRepository : IGenericRepository<Asset>
{
    Task<IEnumerable<Asset>> GetByIdsAsync(params Guid[] ids);
    Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn);
}
