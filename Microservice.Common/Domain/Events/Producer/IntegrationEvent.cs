using MediatR;
using Microservice.Common.Domain.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Microservice.Common.Domain.Events.Producer;
public record IntegrationEvent : INotification, IGetIdentity
{
    public IntegrationEvent(string name, string body, string version = "v1")
    {
        Id = Guid.NewGuid();
        Name = name;
        Version = version;
        Body = body;
    }

    [Key]
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public string Body { get; init; }
    public Uri? Source { get; set; }
    public string? TraceId { get; set; }

    public static IntegrationEvent FromObject(
        object entity,
        string eventType,
        string version = "v1",
        JsonSerializerOptions? jsonOptions = null)
    {
        return new IntegrationEvent(
            CreateEventName(entity, eventType),
            System.Text.Json.JsonSerializer.Serialize(entity, jsonOptions ?? JsonSerializerOptions.Default),
            version);
    }

    private static string CreateEventName(object entity, string eventType)
    {
        return entity.GetType().Name + eventType;
    }
}