using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Models;
using ErrorOr;

namespace AssetAPI.Presentation.Mapping;

public static class AssetMapper
{
    public static ErrorOr<Asset> ToDomain(this AssetDto dto)
    {
        var result = new Asset(dto.Id)
        {
            Name = dto.Name,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
        };
        result.SetPrice(dto.Price);

        return result;
    }

    public static AssetDto ToDto(this Asset asset)
    {
        return new AssetDto
        {
            Id = asset.Id,
            Name = asset.Name,
            ValidFrom = asset.ValidFrom,
            ValidTo = asset.ValidTo,
            Price = asset.Price,
        };
    }
}
