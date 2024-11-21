using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features.Events;
public class PublishEntityIntegrationEventCommand
    (string name, string version, Entity entity)
    : INotification
{
    public string Name { get; set; } = name;
    public string Version { get; set; } = version;
    public Entity Entity { get; set; } = entity;
}
