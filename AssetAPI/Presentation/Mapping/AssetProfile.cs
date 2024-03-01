using AssetAPI.Domain.Models;
using AssetAPI.Presentation.Models;
using AutoMapper;

namespace AssetAPI.Presentation.Mapping;

public class AssetProfile : Profile
{
    public AssetProfile()
    {
        CreateMap<Asset, AssetDto>();
        CreateMap<AssetDto, Asset>();
    }
}
