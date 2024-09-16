using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class UpdateEntityCommand<T>(Guid id, T entity) 
    : IRequest<ErrorOr<Updated>>
    where T : AggregateRoot
{
    public Guid Id { get; set; } = id;
    public T Entity { get; set; } = entity;

}
