using MediatR;
using Microservice.Common.Domain.Models;
using System.ComponentModel.DataAnnotations;

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
}
