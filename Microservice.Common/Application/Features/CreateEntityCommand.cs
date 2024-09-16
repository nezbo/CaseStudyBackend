using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class CreateEntityCommand<T>(T entity) 
    : IRequest<ErrorOr<T>>
    where T : AggregateRoot
{
    public T Entity { get; set; } = entity;
}
