using MediatR;

namespace Microservice.Common.Application.Features;
public class ListEntitiesQuery<T> : IRequest<IEnumerable<T>>
{
    public IEnumerable<Guid> Ids { get; set; }

    public ListEntitiesQuery(IEnumerable<Guid> ids) => Ids = ids;
}
