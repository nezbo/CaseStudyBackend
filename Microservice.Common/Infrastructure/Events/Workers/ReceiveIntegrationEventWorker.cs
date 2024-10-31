using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Microservice.Common.Infrastructure.Events.Workers;
public class ReceiveIntegrationEventWorker<TEvent>
    : IHostedService
{
    private const int POLL_FREQUENCY = 1000;

    private Task? _doWorkTask = null;
    private PeriodicTimer? _timer = null!;
    private readonly CancellationTokenSource _cts = new();

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RabbitMQEventSubscriber _subscriber;
    private readonly IOptions<RabbitMQSettings> _settings;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<ReceiveIntegrationEventWorker<TEvent>> _logger;

    private readonly string _eventKey;

    public ReceiveIntegrationEventWorker(
        string eventKey,
        IServiceScopeFactory serviceScopeFactory,
        RabbitMQEventSubscriber subscriber,
        IOptions<RabbitMQSettings> settings,
        JsonSerializerOptions jsonOptions,
        ILogger<ReceiveIntegrationEventWorker<TEvent>> logger)
    {
        _subscriber = subscriber;
        _settings = settings;
        _jsonOptions = jsonOptions;
        _logger = logger;

        _eventKey = eventKey;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _doWorkTask = ExecuteAsync();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_doWorkTask is null)
            return;

        _cts.Cancel();
        await _doWorkTask;

        _timer?.Dispose();
        _cts.Dispose();
    }

    private async Task ExecuteAsync()
    {
        var eventConsumer = _subscriber.CreateEventConsumer(_settings.Value.ExchangeName, _eventKey);

        eventConsumer.Consumer.Received += async (model, ea) =>
        {
            try
            {
                var evtDataResponse = await ea.ParseEventFromAsync<TEvent>(_jsonOptions);

                var parentContext = RabbitMQDiagnostics.Propagator.Extract(default, ea.BasicProperties, ExtractTraceContextFromBasicProperties);
                Baggage.Current = parentContext.Baggage;

                using (var processingActivity = RabbitMQDiagnostics.ActivitySource.StartActivity("RabbitMq Receive", ActivityKind.Consumer, parentContext.ActivityContext))
                {
                    if (processingActivity != null)
                    {
                        AddActivityTags(processingActivity);
                        processingActivity.AddTag("messaging.eventId", ea.BasicProperties.MessageId);
                        processingActivity.AddTag("messaging.eventType", evtDataResponse.Name);
                        processingActivity.AddTag("messaging.eventVersion", evtDataResponse.Version);
                    }

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>()!;
                        await mediator.Publish(evtDataResponse, _cts.Token);
                    }
                    eventConsumer.Channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                eventConsumer.Channel.BasicReject(ea.DeliveryTag, true);
            }
        };

        while (!_cts.IsCancellationRequested)
        {
            eventConsumer.Channel.BasicConsume(
                _settings.Value.ExchangeName,
                false,
                eventConsumer.Consumer);

            await Task.Delay(POLL_FREQUENCY, _cts.Token);
        }
    }

    private IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
    {
        try
        {
            if (props.Headers.TryGetValue(key, out var value))
            {
                var bytes = value as byte[];
                return [Encoding.UTF8.GetString(bytes!)];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to extract trace context: {ex}");
        }

        return [];
    }

    private static void AddActivityTags(Activity activity)
    {
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination_kind", "queue");
        activity?.SetTag("messaging.rabbitmq.queue", "sample");
    }
}
