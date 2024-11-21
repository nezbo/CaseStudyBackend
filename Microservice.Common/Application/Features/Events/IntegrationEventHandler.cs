using MediatR;
using Microservice.Common.Domain.Events.Consumer;
using Microservice.Common.Domain.Models;
using System.Diagnostics;
using System.Reflection;

namespace Microservice.Common.Application.Features.Events;
public abstract class IntegrationEventHandler<TBody> 
    : INotificationHandler<ReceivedIntegrationEvent<TBody>>
    where TBody : IGetIdentity
{
    protected abstract Task HandleIntegrationEvent(ReceivedIntegrationEvent<TBody> notification, CancellationToken cancellationToken);

    public async Task Handle(ReceivedIntegrationEvent<TBody> intEvent, CancellationToken cancellationToken)
    {
        var attr = GetType().GetCustomAttribute<IntegrationEventHandlerAttribute>();

        if (attr != null
            && intEvent.Name == attr.EventName
            && intEvent.Version == attr.EventVersion)
        {
            using (var activity = Activity.Current?.Source.StartActivity(GetType().Name, ActivityKind.Consumer))
            {
                activity?.AddTag("event.body.type", typeof(TBody).Name);
                activity?.AddTag("event.body.id", intEvent.Body.Id);

                try
                {
                    await HandleIntegrationEvent(intEvent, cancellationToken);
                }
                catch (Exception ex)
                {
                    activity?.AddException(ex);
                }
            }
        }
    }
}
