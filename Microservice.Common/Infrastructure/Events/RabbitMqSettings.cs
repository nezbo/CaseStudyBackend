﻿namespace Microservice.Common.Infrastructure.Events;
public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";

    public string ExchangeName { get; set; } = "";
}