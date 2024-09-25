using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Microservice.Common.Infrastructure.Events;
public class RabbitMQConnection
{
    public RabbitMQConnection(string hostName)
    {
        var connectionRetry = 10;

        while (connectionRetry > 0)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = hostName
                };

                Connection = factory.CreateConnection();
                break;
            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine(e.Message);

                connectionRetry--;

                Task.Delay(TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
            }
        }

        if (Connection is null)
        {
            throw new ConnectionAbortedException(hostName + ": Unable to connect to RabbitMQ");
        }
    }
    public IConnection Connection { get; init; }
}
