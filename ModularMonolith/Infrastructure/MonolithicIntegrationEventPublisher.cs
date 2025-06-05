using MediatR;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Domain.Events.Consumer;
using Microservice.Common.Domain.Events.Producer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ModularMonolith.Infrastructure;

public class MonolithicIntegrationEventPublisher : IIntegrationEventPublisher
{
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
}
