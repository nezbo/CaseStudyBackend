using AssetAPI.Models.Api;
using MediatR;

namespace AssetAPI.Features.Assets.ListAssets
{
    public class ListAssetsQuery : IRequest<IEnumerable<AssetDto>>
    {
        public IEnumerable<Guid> Ids { get; set; }

        public ListAssetsQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
