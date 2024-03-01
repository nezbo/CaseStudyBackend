using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class GetEntityQuery<T> : IRequest<T> where T : IIdentity
{
    public Guid Id { get; set; }

    public GetEntityQuery(Guid id) => Id = id;
}
