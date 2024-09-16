using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class ListAllEntitiesQuery<T> 
    : IRequest<ErrorOr<IEnumerable<T>>>
    where T : AggregateRoot
{
}
