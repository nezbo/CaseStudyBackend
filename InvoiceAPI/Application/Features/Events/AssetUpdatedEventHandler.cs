using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Domain.Events.Consumer;
using Microservice.Common.Extensions;
using MoreLinq;

namespace InvoiceAPI.Application.Features.Events;

[IntegrationEventHandler(EventName = "AssetUpdated")]
public class AssetUpdatedEventHandler(IMediator mediator, IServiceRepository serviceRepository) 
    : IntegrationEventHandler<AssetDto>
{
    private readonly IMediator _mediator = mediator;
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    protected override async Task HandleIntegrationEvent(ReceivedIntegrationEvent<AssetDto> notification, CancellationToken cancellationToken)
    {
        var asset = notification.Body!;
        var services = await _serviceRepository.GetByAssetIdAsync(asset.Id);

        foreach (var service in services)
        {
            service.Name = asset.Name;
            await _mediator.Send(new UpdateEntityCommand<Service>(service.Id, service), cancellationToken);
        }
    }
}
