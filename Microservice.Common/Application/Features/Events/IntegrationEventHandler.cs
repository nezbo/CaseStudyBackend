using MediatR;
using Microservice.Common.Domain.Events.Consumer;
using System.Reflection;

namespace Microservice.Common.Application.Features.Events;
public abstract class IntegrationEventHandler<TBody> : INotificationHandler<ReceivedIntegrationEvent<TBody>>
{
    protected abstract Task HandleIntegrationEvent(ReceivedIntegrationEvent<TBody> notification, CancellationToken cancellationToken);

    public async Task Handle(ReceivedIntegrationEvent<TBody> intEvent, CancellationToken cancellationToken)
    {
        var attr = GetType().GetCustomAttribute<IntegrationEventHandlerAttribute>();

        if (attr != null
            && intEvent.Name == attr.EventName
            && intEvent.Version == attr.EventVersion)
        {
            await HandleIntegrationEvent(intEvent, cancellationToken);
        }
    }
}
