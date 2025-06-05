namespace Microservice.Common.Application.OpenTelemetry.Extensions;
public class ActivitySourceProviderAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
