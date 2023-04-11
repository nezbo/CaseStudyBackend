using MediatR;

namespace Microservice.Common.Features;

public class ListEntitiesQuery<T> : IRequest<IEnumerable<T>>
{
}
