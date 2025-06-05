using MediatR;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.Domain.Events.Consumer;
using Microservice.Common.Domain.Events.Producer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace ModularMonolith.Infrastructure;

[ActivitySourceProvider(nameof(MonolithicIntegrationEventPublisher))]
public class MonolithicIntegrationEventPublisher : IIntegrationEventPublisher
{
    public static readonly ActivitySource ActivitySource = new(nameof(MonolithicIntegrationEventPublisher));

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly Dictionary<(string Name, string Version), Type> _eventTypeMap = [];

    public MonolithicIntegrationEventPublisher(
        IServiceScopeFactory serviceScopeFactory,
        JsonSerializerOptions? serializerOptions = null)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _serializerOptions = serializerOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);

        ScanForIntegrationEventHandlers();
    }

    private void ScanForIntegrationEventHandlers()
    {
        var handlerType = typeof(IntegrationEventHandler<>);
        var attributeType = typeof(IntegrationEventHandlerAttribute);

        // Scan all loaded assemblies for types that inherit from IntegrationEventHandler<>
        var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && t.BaseType != null
                && t.BaseType.IsGenericType
                && t.BaseType.GetGenericTypeDefinition() == handlerType);

        foreach (var type in handlerTypes)
        {
            var attr = type.GetCustomAttributes(attributeType, true)
                           .OfType<IntegrationEventHandlerAttribute>()
                           .FirstOrDefault();
            if (attr == null) continue;

            var bodyType = type.BaseType!.GetGenericArguments()[0];
            var key = (attr.EventName, attr.EventVersion);
            _eventTypeMap.TryAdd(key, bodyType);
        }
    }

    public async Task PublishAsync(IntegrationEvent integrationEvent)
    {
        var key = (integrationEvent.Name, integrationEvent.Version);
        if (_eventTypeMap.TryGetValue(key, out var bodyType))
        {
            ActivityContext parentContext = ExtractActivityContext(integrationEvent);
            using var activity = ActivitySource.StartActivity(nameof(PublishAsync), ActivityKind.Consumer, parentContext);
            activity?.AddTag("messaging.eventId", integrationEvent.Id);
            activity?.AddTag("messaging.eventType", integrationEvent.Name);
            activity?.AddTag("messaging.eventVersion", integrationEvent.Version);

            using var scope = _serviceScopeFactory.CreateScope();
            var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var dbContexts = scope.ServiceProvider.GetServices<DbContext>();

            // Deserialize the body to the correct type
            var body = JsonSerializer.Deserialize(integrationEvent.Body, bodyType, _serializerOptions)
                       ?? throw new InvalidOperationException($"Failed to deserialize event body for {key}");

            // Construct ReceivedIntegrationEvent<TBody> dynamically
            var receivedEventType = typeof(ReceivedIntegrationEvent<>).MakeGenericType(bodyType);
            var receivedEvent = Activator.CreateInstance(
                receivedEventType,
                $"{integrationEvent.Name}.{integrationEvent.Version}",
                body
            ) ?? throw new InvalidOperationException("Failed to create ReceivedIntegrationEvent instance");

            await scopedMediator.Publish((INotification)receivedEvent);

            // Save any changes to the DbContexts
            foreach (var dbContext in dbContexts)
            {
                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }

    private static ActivityContext ExtractActivityContext(IntegrationEvent integrationEvent)
    {
        // Create parentContext from integrationEvent.TraceId
        ActivityContext parentContext = default;
        if (!string.IsNullOrWhiteSpace(integrationEvent.TraceId))
        {
            // Try to parse the TraceId as W3C traceparent header format
            // If not, fallback to using it as a TraceId only
            if (ActivityContext.TryParse(integrationEvent.TraceId, null, out var parsedContext))
            {
                parentContext = parsedContext;
            }
            else
            {
                // Try to parse as just a TraceId (hex string)
                var traceId = ActivityTraceId.CreateFromString(integrationEvent.TraceId);
                parentContext = new ActivityContext(
                        traceId,
                        ActivitySpanId.CreateRandom(),
                        ActivityTraceFlags.Recorded);
            }
        }

        return parentContext;
    }
}
