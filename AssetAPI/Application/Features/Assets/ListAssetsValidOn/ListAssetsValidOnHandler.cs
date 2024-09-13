using AssetAPI.Application.Repository;
using AssetAPI.Presentation.Models;
using AutoMapper;
using MediatR;

namespace AssetAPI.Application.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnHandler : IRequestHandler<ListAssetsValidOnQuery, IEnumerable<AssetDto>>
{
    private readonly IAssetRepository _repository;
    private readonly IMapper _mapper;

    public ListAssetsValidOnHandler(IAssetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AssetDto>> Handle(ListAssetsValidOnQuery request, CancellationToken cancellationToken)
    {
        var matches = await _repository.GetValidOnAsync(request.ValidOn);
        return matches.Select(o => _mapper.Map<AssetDto>(o));
    }
}
