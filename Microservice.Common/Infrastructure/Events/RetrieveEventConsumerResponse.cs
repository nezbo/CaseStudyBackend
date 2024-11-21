using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Common.Infrastructure.Events;
public class RetrieveEventConsumerResponse(IChannel channel)
{
    public AsyncEventingBasicConsumer Consumer { get; private set; } = new AsyncEventingBasicConsumer(channel);

    public IChannel Channel { get; private set; } = channel;
}