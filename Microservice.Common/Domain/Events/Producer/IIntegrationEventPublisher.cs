namespace Microservice.Common.Domain.Events.Producer;
public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}
