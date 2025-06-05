using InvoiceAPI.Application.External.Models;

namespace InvoiceAPI.Application.External;

public interface IAssetService
{
    Task<AssetDto?> GetAssetAsync(Guid id);
    Task<IEnumerable<AssetDto>> GetAssetsAsync(params IEnumerable<Guid> ids);
    Task<IEnumerable<AssetDto>> GetAssetsAsync();
}
