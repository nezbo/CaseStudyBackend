namespace Microservice.Common.SourceGeneration.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GenerateDtoAttribute(string? @namespace = null)
        : Attribute
{
    public string? Namespace { get; set; } = @namespace;
}
