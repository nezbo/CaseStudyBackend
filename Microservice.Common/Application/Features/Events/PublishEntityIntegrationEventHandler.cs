using MediatR;
using Microservice.Common.Domain.Events.Producer;
using System.Text.Json;

namespace Microservice.Common.Application.Features.Events;
public class PublishEntityIntegrationEventHandler
    (IMediator mediator, JsonSerializerOptions jsonOptions)
    : INotificationHandler<PublishEntityIntegrationEventCommand>
{
    public Task Handle(PublishEntityIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        return mediator.Publish(IntegrationEvent.FromObject(request.Entity, request.Name, request.Version, jsonOptions), cancellationToken);
    }
}
