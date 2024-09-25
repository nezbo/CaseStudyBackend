using CloudNative.CloudEvents;
using CloudNative.CloudEvents.NewtonsoftJson;
using Microservice.Common.Domain.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;

namespace Microservice.Common.Infrastructure.Events;
public class RabbitMQEventPublisher : IIntegrationEventPublisher
{
    private readonly IOptions<RabbitMqSettings> _settings;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQEventPublisher> _logger;

    public RabbitMQEventPublisher(
        IOptions<RabbitMqSettings> settings,
        RabbitMQConnection connection,
        ILogger<RabbitMQEventPublisher> logger)
    {
        _settings = settings;
        _connection = connection.Connection;
        _logger = logger;
    }

    public Task PublishAsync(IntegrationEvent evt)
    {
        using (Activity? activity = RabbitMqDiagnostics.ActivitySource.StartActivity("RabbitMq Publish", ActivityKind.Producer, parentId: evt.TraceId))
        {
            var channel = _connection.CreateModel();
            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.ExchangeDeclare(exchange: _settings.Value.ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);

            var queueName = $"{evt.Name}.{evt.Version}";

            channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);

            if (activity != null)
            {
                AddActivityToHeader(activity, props);
                activity.AddTag("messaging.eventId", evt.Id);
                activity.AddTag("messaging.eventType", evt.Name);
                activity.AddTag("messaging.eventVersion", evt.Version);
                activity.AddTag("messaging.eventSource", evt.Source);
            }

            var evtWrapper = new CloudEvent
            {
                Type = queueName,
                Source = evt.Source ?? new Uri("http://example.com"),
                Time = DateTimeOffset.Now,
                DataContentType = MediaTypeNames.Application.Json,
                Id = evt.Id.ToString(),
                Data = evt.Body,
            };

            if (evt.TraceId != null)
                evtWrapper.SetAttributeFromString("traceparent", evt.TraceId);

            this._logger.LogInformation("Publishing event {EventId} {EventVersion} with traceId {TraceId}", evtWrapper.Id, evt.Version, evt.TraceId);

            var evtFormatter = new JsonEventFormatter();

            var json = evtFormatter.ConvertToJObject(evtWrapper).ToString();

            _logger.LogInformation(json);

            var body = Encoding.UTF8.GetBytes(json);
            this._logger.LogInformation($"Publishing '{queueName}' to '{_settings.Value.ExchangeName}'");

            channel.BasicPublish(
                exchange: _settings.Value.ExchangeName,
                routingKey: queueName,
                basicProperties: props,
                body: body);
        }

        return Task.CompletedTask;
    }

    private void AddActivityToHeader(Activity activity, IBasicProperties props)
    {
        RabbitMqDiagnostics.Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props, InjectContextIntoHeader);
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination_kind", "queue");
        activity?.SetTag("messaging.rabbitmq.queue", "sample");
    }

    private void InjectContextIntoHeader(IBasicProperties props, string key, string value)
    {
        try
        {
            props.Headers ??= new Dictionary<string, object>();
            props.Headers[key] = value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to inject trace context.");
        }
    }
}
