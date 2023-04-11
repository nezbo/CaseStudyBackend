using AssetAPI.Models.Api;
using MediatR;

namespace AssetAPI.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnQuery : IRequest<IEnumerable<AssetDto>>
{
    public DateOnly ValidOn { get; set; }

    public ListAssetsValidOnQuery(DateOnly validOn)
    {
        ValidOn = validOn;
    }
}
