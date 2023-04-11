using MediatR;
using Microservice.Common.Models;

namespace Microservice.Common.CQRS;

public class GetEntityQuery<T> : IRequest<T> where T : IIdentity
{
    public Guid Id { get; set; }

    public GetEntityQuery(Guid id)
    {
        Id = id;
    }
}
