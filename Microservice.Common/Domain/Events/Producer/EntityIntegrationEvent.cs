using Newtonsoft.Json;

namespace Microservice.Common.Domain.Events.Producer;
public record EntityIntegrationEvent : IntegrationEvent
{
    public EntityIntegrationEvent(object entity, string eventType)
        : base(CreateEventName(entity, eventType), SerializeEntity(entity))
    {
    }

    private static string SerializeEntity(object entity)
    {
        return JsonConvert.SerializeObject(entity);
    }

    private static string CreateEventName(object entity, string eventType)
    {
        return entity.GetType().Name + eventType;
    }
}
