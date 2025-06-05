using Microservice.Common.Application.OpenTelemetry.Extensions;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace Microservice.Common.Infrastructure.Events;

[ActivitySourceProvider("RabbitMQ")]
public static class RabbitMQDiagnostics
{
    public static readonly ActivitySource ActivitySource = new("RabbitMQ");
    public static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    public static readonly string HEADER_DATA_ID = "dataid";
}
