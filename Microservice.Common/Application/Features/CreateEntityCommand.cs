using ErrorOr;
using MediatR;

namespace Microservice.Common.Application.Features;

public class CreateEntityCommand<T>(T entity) : IRequest<ErrorOr<T>>
{
    public T Entity { get; set; } = entity;
}
