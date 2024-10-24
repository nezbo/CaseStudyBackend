using MediatR;

namespace Microservice.Common.Application.Features.Events;
public class PublishEntityIntegrationEventCommand
    (string name, string version, object entity)
    : INotification
{
    public string Name { get; set; } = name;
    public string Version { get; set; } = version;
    public object Entity { get; set; } = entity;
}
