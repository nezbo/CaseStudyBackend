using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class DeleteEntityCommand<T> : IRequest<bool>
    where T : IIdentity
{
    public Guid Id { get; set; }

    public DeleteEntityCommand(Guid id) => Id = id;
}
