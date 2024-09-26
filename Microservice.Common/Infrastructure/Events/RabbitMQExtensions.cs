using CloudNative.CloudEvents.NewtonsoftJson;
using Microservice.Common.Domain.Events.Consumer;
using RabbitMQ.Client.Events;
using System.Net.Mime;

namespace Microservice.Common.Infrastructure.Events;
public static class RabbitMQExtensions
{
    public static async Task<ReceivedIntegrationEvent<TBody>> ParseEventFromAsync<TBody>(this BasicDeliverEventArgs rabbitMqEvent)
    {
        var formatter = new JsonEventFormatter<TBody>();
        var evtWrapper = await formatter.DecodeStructuredModeMessageAsync(
            new MemoryStream(rabbitMqEvent.Body.ToArray()),
            new ContentType(MediaTypeNames.Application.Json),
            null);

        return new ReceivedIntegrationEvent<TBody>(rabbitMqEvent.RoutingKey, (TBody?)evtWrapper.Data);
    }
}
