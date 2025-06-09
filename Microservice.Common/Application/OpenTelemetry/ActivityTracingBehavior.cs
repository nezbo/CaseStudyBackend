using MediatR;
using System.Diagnostics;
using System.Text;

namespace Microservice.Common.Application.OpenTelemetry.Extensions;

/// <summary>
/// MediatR pipeline behavior that creates an Activity for each request/handler invocation.
/// </summary>
public class ActivityTracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var activityName = $"MediatR.{GetSimpleTypeName(typeof(TRequest))}";
        using var activity = Activity.Current?.Source.StartActivity(activityName, ActivityKind.Internal);

        activity?.SetTag("mediatr.request_type", GetSimpleTypeName(typeof(TRequest)));
        activity?.SetTag("mediatr.response_type", GetSimpleTypeName(typeof(TResponse)));

        return await next();
    }

    private static string GetSimpleTypeName(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var sb = new StringBuilder();
        var name = type.Name;
        var index = name.IndexOf('`');
        sb.Append(index > 0 ? name[..index] : name);
        sb.Append('<');
        var args = type.GetGenericArguments();
        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(GetSimpleTypeName(args[i]));
        }
        sb.Append('>');
        return sb.ToString();
    }
}
