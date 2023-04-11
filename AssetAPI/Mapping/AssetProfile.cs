using AssetAPI.Models.Api;
using AssetAPI.Models.Database;
using AutoMapper;

namespace AssetAPI.Mapping;

public class AssetProfile : Profile
{
    public AssetProfile()
	{
        CreateMap<Asset, AssetDto>();
        CreateMap<AssetDto, Asset>();
	}
}
