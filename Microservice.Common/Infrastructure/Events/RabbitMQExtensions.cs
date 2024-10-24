using CloudNative.CloudEvents.SystemTextJson;
using ErrorOr;
using Microservice.Common.Domain.Events.Consumer;
using RabbitMQ.Client.Events;
using System.Net.Mime;
using System.Text.Json;

namespace Microservice.Common.Infrastructure.Events;
public static class RabbitMQExtensions
{
    public static async Task<ReceivedIntegrationEvent<TBody>> ParseEventFromAsync<TBody>(
        this BasicDeliverEventArgs rabbitMqEvent,
        JsonSerializerOptions serializerOptions)
    {
        var formatter = new JsonEventFormatter(serializerOptions, new JsonDocumentOptions());
        var evtWrapper = await formatter.DecodeStructuredModeMessageAsync(
            new MemoryStream(rabbitMqEvent.Body.ToArray()),
            new ContentType("application/cloudevents+json; charset=utf-8"),
            null);
        var data = JsonSerializer.Deserialize<TBody>(evtWrapper.Data!.ToString()!, serializerOptions);

        return new ReceivedIntegrationEvent<TBody>(rabbitMqEvent.RoutingKey, data);
    }
}
