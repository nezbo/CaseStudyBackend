namespace Microservice.Common.SourceGeneration.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GenerateCRUDRequestHandlersAttribute(string @namespace = "") 
    : Attribute
{
    public string Namespace { get; set; } = @namespace;
}
