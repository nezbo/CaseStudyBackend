using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace Microservice.Common.Infrastructure.Events;

public static class RabbitMQDiagnostics
{
    public static readonly string ActivitySourceName = "RabbitMQ";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    public static readonly string HEADER_DATA_ID = "dataid";
}
