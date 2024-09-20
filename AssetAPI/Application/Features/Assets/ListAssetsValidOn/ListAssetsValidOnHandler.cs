using AssetAPI.Application.Repository;
using AssetAPI.Domain.Models;
using ErrorOr;
using MediatR;

namespace AssetAPI.Application.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnHandler(IAssetRepository repository) 
    : IRequestHandler<ListAssetsValidOnQuery, ErrorOr<IEnumerable<Asset>>>
{
    private readonly IAssetRepository _repository = repository;

    public async Task<ErrorOr<IEnumerable<Asset>>> Handle(ListAssetsValidOnQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetValidOnAsync(request.ValidOn);
    }
}
