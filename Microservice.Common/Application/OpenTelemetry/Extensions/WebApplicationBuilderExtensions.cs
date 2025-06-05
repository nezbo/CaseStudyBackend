using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;
using PackageOpenTelemetry = OpenTelemetry;

namespace Microservice.Common.Application.OpenTelemetry.Extensions;
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, string serviceName, string otlpEndpointUrl, params IEnumerable<Assembly> assemblies)
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(serviceName, serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString());
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSourcesFromAssemblies(assemblies)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Protocol = PackageOpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        options.Endpoint = new Uri(otlpEndpointUrl);
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Protocol = PackageOpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        options.Endpoint = new Uri(otlpEndpointUrl);
                    });
            })
            .WithLogging(logging =>
            {
                logging.AddOtlpExporter(options =>
                {
                    options.Protocol = PackageOpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    options.Endpoint = new Uri(otlpEndpointUrl);
                });
            });

        return builder;
    }
}
