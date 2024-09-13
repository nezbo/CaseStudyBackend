using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class GetEntityQuery<T>(Guid id) : IRequest<ErrorOr<T>> where T : IIdentity
{
    public Guid Id { get; set; } = id;
}
