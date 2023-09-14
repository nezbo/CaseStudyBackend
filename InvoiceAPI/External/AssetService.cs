using InvoiceAPI.External.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InvoiceAPI.External;

public class AssetService : IAssetService
{
    private readonly string _rootUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AssetService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IOptions<JsonOptions> jsonOptions)
    {
        _rootUrl = configuration["ASSET_API_URL"];
        _httpClient = httpClientFactory.CreateClient();
        _jsonOptions = jsonOptions.Value.SerializerOptions;
    }

    public async Task<AssetDto?> GetAssetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<AssetDto>($"{_rootUrl}/api/v1/asset/{id}", _jsonOptions);
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AssetDto>>($"{_rootUrl}/api/v1/asset", _jsonOptions) 
            ?? Enumerable.Empty<AssetDto>();
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync(params Guid[] ids)
    {
        var query = string.Join('&', ids.Select(id => $"ids={id}"));
        var url = $"{_rootUrl}/api/v1/asset?{query}";
        return await _httpClient.GetFromJsonAsync<IEnumerable<AssetDto>>(url, _jsonOptions)
            ?? Enumerable.Empty<AssetDto>();
    }
}
