using AssetAPI.Domain.Models;
using Microservice.Common.Application.Repository;

namespace AssetAPI.Application.Repository;

public interface IAssetRepository : IGenericRepository<Asset>
{
    Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn);
}
