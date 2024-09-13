using ErrorOr;
using MediatR;

namespace Microservice.Common.Application.Features;

public class UpdateEntityCommand<T>(Guid id, T entity) : IRequest<ErrorOr<Updated>>
{
    public Guid Id { get; set; } = id;
    public T Entity { get; set; } = entity;

}
