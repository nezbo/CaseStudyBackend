using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Models;
using ErrorOr;

namespace AssetAPI.Presentation.Mapping;

public static class AssetMapper
{
    public static ErrorOr<Asset> ToDomain(this AssetDto dto) 
        => Asset.Create(dto.Id, dto.Name, dto.Price, dto.ValidFrom, dto.ValidTo);

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
