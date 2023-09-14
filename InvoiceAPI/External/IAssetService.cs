using InvoiceAPI.External.Models;

namespace InvoiceAPI.External;

public interface IAssetService
{
    Task<AssetDto?> GetAssetAsync(Guid id);
    Task<IEnumerable<AssetDto>> GetAssetsAsync();
}
