namespace Microservice.Common.Application.Features.Events;

[AttributeUsage(AttributeTargets.Class)]
public class IntegrationEventHandlerAttribute : Attribute
{
    public required string EventName { get; set; }
    public string EventVersion { get; set; } = "v1";
}
