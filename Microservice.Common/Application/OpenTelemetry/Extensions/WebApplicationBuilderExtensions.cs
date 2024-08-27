using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace Microservice.Common.Application.OpenTelemetry.Extensions;
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, string? serviceName = null)
    {
        serviceName ??= Assembly.GetExecutingAssembly().FullName!;

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(serviceName, serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString());
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(builder.Configuration.GetValue<string>("OTLP_Endpoint")!);
                    });
            });

        return builder;
    }
}
