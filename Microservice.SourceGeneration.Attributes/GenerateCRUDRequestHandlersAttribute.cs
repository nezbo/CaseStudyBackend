using System;

namespace Microservice.SourceGeneration.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateCRUDRequestHandlersAttribute : Attribute
    {
    }
}
