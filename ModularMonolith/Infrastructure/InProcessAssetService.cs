using AssetAPI.Presentation.Controllers;
using InvoiceAPI.Application.External;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Asset = InvoiceAPI.Application.External.Models.AssetDto;
using AssetModel = AssetAPI.Presentation.Models.AssetDto;

namespace ModularMonolith.Infrastructure;

[ActivitySourceProvider(nameof(InProcessAssetService))]
public class InProcessAssetService(AssetController assetController) : IAssetService
{
    public static readonly ActivitySource ActivitySource = new(nameof(InProcessAssetService));

    private readonly AssetController _assetController = assetController;

    public async Task<Asset?> GetAssetAsync(Guid id)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetAssetAsync));
        var result = await _assetController.Read(id);
        if (result is ObjectResult objectResult && objectResult.Value is AssetModel dto)
            return Map(dto).Single();

        if (result is OkObjectResult okResult && okResult.Value is AssetModel okDto)
            return Map(okDto).Single();

        return null;
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync(params IEnumerable<Guid> ids)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetAssetsAsync));
        var result = await _assetController.List(ids);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync()
    {
        using var activity = ActivitySource.StartActivity(nameof(GetAssetsAsync));
        var result = await _assetController.List([]);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    public async Task<IEnumerable<Asset>> GetAssetsValidOnAsync(DateOnly validOn)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetAssetsValidOnAsync));
        var result = await _assetController.List(validOn);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    private static IEnumerable<Asset> Map(params IEnumerable<AssetModel> okDtos)
    {
        foreach (var asset in okDtos)
        {
            yield return new Asset
            {
                Id = asset.Id,
                Name = asset.Name,
                Price = asset.Price,
                ValidFrom = asset.ValidFrom,
                ValidTo = asset.ValidTo
            };
        }
    }
}
