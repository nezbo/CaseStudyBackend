namespace Microservice.Common.Infrastructure.Events;
public class RabbitMQSettings
{
    public string HostName { get; set; } = "localhost";

    public string ExchangeName { get; set; } = "";
}