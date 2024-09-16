using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;
public class ListEntitiesQuery<T>(IEnumerable<Guid> ids) 
    : IRequest<ErrorOr<IEnumerable<T>>>
    where T : AggregateRoot
{
    public IEnumerable<Guid> Ids { get; set; } = ids;
}
