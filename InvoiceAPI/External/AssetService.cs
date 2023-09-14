using InvoiceAPI.External.Models;
using System.Text.Json;

namespace InvoiceAPI.External;

public class AssetService : IAssetService
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;

    public AssetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AssetDto?> GetAssetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<AssetDto>($"api/v1/asset/{id}", JSON_OPTIONS);
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AssetDto>>("api/v1/asset", JSON_OPTIONS) 
            ?? Enumerable.Empty<AssetDto>();
    }
}
