using MediatR;
using Microservice.Common.Models;

namespace Microservice.Common.CQRS;

public class DeleteEntityCommand<T> : IRequest<bool> where T : IIdentity
{
    public Guid Id { get; set; }

    public DeleteEntityCommand(Guid id) => Id = id;
}
