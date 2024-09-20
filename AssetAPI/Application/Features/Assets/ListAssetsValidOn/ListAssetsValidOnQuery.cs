using AssetAPI.Domain.Models;
using ErrorOr;
using MediatR;

namespace AssetAPI.Application.Features.Assets.ListAssetsValidOn;

public class ListAssetsValidOnQuery(DateOnly validOn) 
    : IRequest<ErrorOr<IEnumerable<Asset>>>
{
    public DateOnly ValidOn { get; set; } = validOn;
}
