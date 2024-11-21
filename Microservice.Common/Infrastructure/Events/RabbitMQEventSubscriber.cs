using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microservice.Common.Infrastructure.Events;
public class RabbitMQEventSubscriber(RabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
{
    private readonly RabbitMQConnection _connection = connection;
    private readonly IOptions<RabbitMQSettings> _settings = settings;

    public async Task<RetrieveEventConsumerResponse> CreateEventConsumerAsync(string queueName, string eventName, int deliveryLimit = 3)
    {
        string dqlExchangeName = $"{_settings.Value.ExchangeName}-dlq";
        var channel = await this._connection.Connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: _settings.Value.ExchangeName, ExchangeType.Topic, durable: true);
        await channel.ExchangeDeclareAsync(exchange: dqlExchangeName, ExchangeType.Direct, durable: true);

        var dlq = await channel.QueueDeclareAsync($"{queueName}-dlq", durable: true, autoDelete: false, exclusive: false,
            arguments: new Dictionary<string, object?>
            {
                { "x-queue-type", "quorum" }
            });

        var queue = await channel.QueueDeclareAsync(queueName, durable: true, autoDelete: false, exclusive: false,
            arguments: new Dictionary<string, object?>
            {
                {"x-queue-type", "quorum"},
                {"x-delivery-limit", deliveryLimit},
                {"x-dead-letter-exchange", dqlExchangeName},
                {"x-dead-letter-routing-key", dlq.QueueName}
            });

        await channel.QueueBindAsync(dlq, exchange: dqlExchangeName, routingKey: dlq.QueueName);
        await channel.QueueBindAsync(queue, exchange: _settings.Value.ExchangeName, routingKey: eventName);

        return new RetrieveEventConsumerResponse(channel);
    }
}
