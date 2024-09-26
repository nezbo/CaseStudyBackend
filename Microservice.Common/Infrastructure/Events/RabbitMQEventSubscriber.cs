using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microservice.Common.Infrastructure.Events;
public class RabbitMQEventSubscriber(RabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
{
    private readonly RabbitMQConnection _connection = connection;
    private readonly IOptions<RabbitMQSettings> _settings = settings;

    public RetrieveEventConsumerResponse CreateEventConsumer(string queueName, string eventName, int deliveryLimit = 3)
    {
        string dqlExchangeName = $"{_settings.Value.ExchangeName}-dlq";
        var channel = this._connection.Connection.CreateModel();
        channel.ExchangeDeclare(exchange: _settings.Value.ExchangeName, ExchangeType.Topic, durable: true);
        channel.ExchangeDeclare(exchange: dqlExchangeName, ExchangeType.Direct, durable: true);

        var dlq = channel.QueueDeclare($"{queueName}-dlq", durable: true, autoDelete: false, exclusive: false,
            arguments: new Dictionary<string, object>
            {
                { "x-queue-type", "quorum" }
            });

        var queue = channel.QueueDeclare(queueName, durable: true, autoDelete: false, exclusive: false,
            arguments: new Dictionary<string, object>
            {
                {"x-queue-type", "quorum"},
                {"x-delivery-limit", deliveryLimit},
                {"x-dead-letter-exchange", dqlExchangeName},
                {"x-dead-letter-routing-key", dlq.QueueName}
            });

        channel.QueueBind(dlq, exchange: dqlExchangeName, routingKey: dlq.QueueName);
        channel.QueueBind(queue, exchange: _settings.Value.ExchangeName, routingKey: eventName);

        return new RetrieveEventConsumerResponse(channel);
    }
}
