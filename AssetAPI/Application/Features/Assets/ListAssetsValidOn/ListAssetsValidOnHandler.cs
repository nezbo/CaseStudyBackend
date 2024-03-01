using AssetAPI.Application.Repository;
using AssetAPI.Presentation.Models;
using AutoMapper;
using MediatR;

namespace AssetAPI.Application.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnHandler : IRequestHandler<ListAssetsValidOnQuery, IEnumerable<AssetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListAssetsValidOnHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AssetDto>> Handle(ListAssetsValidOnQuery request, CancellationToken cancellationToken)
    {
        var matches = await _unitOfWork.AssetRepository.GetValidOnAsync(request.ValidOn);
        return matches.Select(o => _mapper.Map<AssetDto>(o));
    }
}
