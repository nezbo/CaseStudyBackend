using MediatR;

namespace Microservice.Common.CQRS;

public class CreateEntityCommand<T>  : IRequest<Guid>
{
    public T Entity { get; set; }

    public CreateEntityCommand(T entity) => Entity = entity;
}
