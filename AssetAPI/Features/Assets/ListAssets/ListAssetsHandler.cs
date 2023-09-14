using AssetAPI.Models.Api;
using AssetAPI.Persistence;
using AutoMapper;
using MediatR;

namespace AssetAPI.Features.Assets.ListAssets;

public class ListAssetsHandler : IRequestHandler<ListAssetsQuery, IEnumerable<AssetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListAssetsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AssetDto>> Handle(ListAssetsQuery request, CancellationToken cancellationToken)
    {
        var assets = await _unitOfWork.AssetRepository.GetByIdsAsync(request.Ids.ToArray());
        return _mapper.Map<IEnumerable<AssetDto>>(assets);
    }
}
