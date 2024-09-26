using InvoiceAPI.Application.External.Models;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Domain.Events.Consumer;

namespace InvoiceAPI.Application.Features.Events;

[IntegrationEventHandler(EventName = "AssetUpdated")]
public class AssetUpdatedEventHandler : IntegrationEventHandler<AssetDto>
{
    protected override Task HandleIntegrationEvent(ReceivedIntegrationEvent<AssetDto> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
