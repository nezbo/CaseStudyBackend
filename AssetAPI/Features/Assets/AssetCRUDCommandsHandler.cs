using AssetAPI.Models.Api;
using AssetAPI.Models.Database;
using AutoMapper;
using Microservice.Common.Features;
using Microservice.Common.Repository;

namespace AssetAPI.Features.Assets;

public class AssetCRUDCommandsHandler : BasicCRUDCommandsHandler<AssetDto, Asset>
{
    public AssetCRUDCommandsHandler(IGenericUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}
