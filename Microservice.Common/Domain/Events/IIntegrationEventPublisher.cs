namespace Microservice.Common.Domain.Events;
public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}
