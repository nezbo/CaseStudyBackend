using OpenTelemetry.Trace;
using System.Reflection;

namespace Microservice.Common.Application.OpenTelemetry.Extensions;
public static class OpenTelemetryExtensions
{
    public static string GetActivitySourceName<T>()
    {
        return typeof(T).FullName ?? typeof(T).Name;
    }

    public static TracerProviderBuilder AddSourcesFromAssemblies(this TracerProviderBuilder builder, params IEnumerable<Assembly> assemblies)
    {
        var activitySourceNames = assemblies.SelectMany(a => a.DefinedTypes)
            .Select(t => t.GetCustomAttribute<ActivitySourceProviderAttribute>()?.Name ?? "")
            .Where(name => !string.IsNullOrEmpty(name));

        builder.AddSource([.. activitySourceNames]);

        return builder;
    }
}
