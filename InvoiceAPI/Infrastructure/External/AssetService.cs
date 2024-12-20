﻿using InvoiceAPI.Application.External;
using InvoiceAPI.Application.External.Models;
using Microservice.Common.Application.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using MoreLinq;
using RestSharp;
using RestSharp.Serializers.Json;

namespace InvoiceAPI.Infrastructure.External;

public class AssetService : IAssetService
{
    private RestClient _restClient;

    public AssetService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IOptions<JsonOptions> jsonOptions)
    {
        var baseUrl = configuration["ASSET_API_URL"] ?? throw new ArgumentNullException(nameof(configuration));
        var httpClient = httpClientFactory.CreateClient();
        var serializerOptions = jsonOptions.Value.SerializerOptions;

        var restOptions = new RestClientOptions(baseUrl);
        _restClient = new RestClient(httpClient,
            restOptions,
            configureSerialization: s => s.UseSystemTextJson(serializerOptions));
    }

    public async Task<AssetDto?> GetAssetAsync(Guid id)
    {
        return await _restClient.GetAsync<AssetDto>("api/v1/asset/{id}", new { id });
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync()
    {
        return await _restClient.GetAsync<IEnumerable<AssetDto>>("api/v1/asset")
            ?? [];
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync(params Guid[] ids)
    {
        var request = new RestRequest("api/v1/asset", Method.Get);
        ids.ForEach(id => request.AddQueryParameter("ids", id.ToString()));

        return await _restClient.GetAsync<IEnumerable<AssetDto>>(request)
            ?? [];
    }
}
