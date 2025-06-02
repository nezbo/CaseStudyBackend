using AssetAPI.Presentation.Controllers;
using InvoiceAPI.Application.External;
using Microsoft.AspNetCore.Mvc;
using AssetDto = InvoiceAPI.Application.External.Models.AssetDto;
using AssetModel = AssetAPI.Presentation.Models.AssetDto;

namespace ModularMonolith.Infrastructure;

public class InProcessAssetService(AssetController assetController) : IAssetService
{
    private readonly AssetController _assetController = assetController;

    public async Task<AssetDto?> GetAssetAsync(Guid id)
    {
        // Use the base CRUDController's GetById endpoint
        var result = await _assetController.Read(id);
        if (result is ObjectResult objectResult && objectResult.Value is AssetModel dto)
            return Map(dto).Single();

        if (result is OkObjectResult okResult && okResult.Value is AssetModel okDto)
            return Map(okDto).Single();

        return null;
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync(params Guid[] ids)
    {
        // Use the base CRUDController's GetByIds endpoint
        var result = await _assetController.List(ids);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    public async Task<IEnumerable<AssetDto>> GetAssetsAsync()
    {
        // Use the base CRUDController's GetAll endpoint
        var result = await _assetController.List([]);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    // If you want to expose the ValidOn endpoint as well:
    public async Task<IEnumerable<AssetDto>> GetAssetsValidOnAsync(DateOnly validOn)
    {
        var result = await _assetController.List(validOn);
        if (result is ObjectResult objectResult && objectResult.Value is IEnumerable<AssetModel> dtos)
            return Map(dtos);

        if (result is OkObjectResult okResult && okResult.Value is IEnumerable<AssetModel> okDtos)
            return Map(okDtos);

        return [];
    }

    private static IEnumerable<AssetDto> Map(params IEnumerable<AssetModel> okDtos)
    {
        foreach (var asset in okDtos)
        {
            yield return new AssetDto
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
