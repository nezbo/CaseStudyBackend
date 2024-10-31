using MediatR;

namespace Microservice.Common.Domain.Events.Consumer;
public record ReceivedIntegrationEvent<TBody> : INotification
{
    public ReceivedIntegrationEvent(string NameVersion, TBody body)
    {
        var split = NameVersion.Split('.');
        Name = split[0];
        Version = split[1];
        Body = body;
    }

    public string Name { get; set; }
    public string Version { get; set; }
    public TBody Body { get; set; }
}
