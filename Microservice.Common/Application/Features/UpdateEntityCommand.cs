using MediatR;

namespace Microservice.Common.Application.Features;

public class UpdateEntityCommand<T> : IRequest
{
    public T Entity { get; set; }

    public UpdateEntityCommand(T entity) => Entity = entity;
}
