using AssetAPI.Domain.Models;
using ErrorOr;
using Microservice.Common.Application.Repository;

namespace AssetAPI.Application.Repository;

public interface IAssetRepository : IGenericRepository<Asset>
{
    Task<ErrorOr<IEnumerable<Asset>>> GetValidOnAsync(DateOnly validOn);
}
