using ErrorOr;
using MediatR;

namespace Microservice.Common.Application.Features;

public class ListAllEntitiesQuery<T> : IRequest<ErrorOr<IEnumerable<T>>>
{
}
