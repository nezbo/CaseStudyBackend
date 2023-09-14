using MediatR;

namespace Microservice.Common.Features;

public class ListAllEntitiesQuery<T> : IRequest<IEnumerable<T>>
{
}
