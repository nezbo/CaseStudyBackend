using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Common.Infrastructure.Events;
public class RetrieveEventConsumerResponse(IModel channel)
{
    public EventingBasicConsumer Consumer { get; private set; } = new EventingBasicConsumer(channel);

    public IModel Channel { get; private set; } = channel;
}