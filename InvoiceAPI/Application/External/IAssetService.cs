using InvoiceAPI.Application.External.Models;

namespace InvoiceAPI.Application.External;

public interface IAssetService
{
    Task<AssetDto?> GetAssetAsync(Guid id);
    Task<IEnumerable<AssetDto>> GetAssetsAsync(params Guid[] ids);
    Task<IEnumerable<AssetDto>> GetAssetsAsync();
}
