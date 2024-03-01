using AssetAPI.Presentation.Models;
using MediatR;

namespace AssetAPI.Application.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnQuery : IRequest<IEnumerable<AssetDto>>
{
    public DateOnly ValidOn { get; set; }

    public ListAssetsValidOnQuery(DateOnly validOn)
    {
        ValidOn = validOn;
    }
}
