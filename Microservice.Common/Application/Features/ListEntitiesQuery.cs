using ErrorOr;
using MediatR;

namespace Microservice.Common.Application.Features;
public class ListEntitiesQuery<T>(IEnumerable<Guid> ids) 
    : IRequest<ErrorOr<IEnumerable<T>>>
{
    public IEnumerable<Guid> Ids { get; set; } = ids;
}
