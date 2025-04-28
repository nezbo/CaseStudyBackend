namespace Microservice.Common.SourceGeneration.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class GenerateDtoPropertyAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
