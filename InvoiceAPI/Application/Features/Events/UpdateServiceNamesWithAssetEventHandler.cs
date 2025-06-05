using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Application.Repository;
using InvoiceAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Domain.Events.Consumer;

namespace InvoiceAPI.Application.Features.Events;

[IntegrationEventHandler(EventName = "AssetUpdated")]
public class UpdateServiceNamesWithAssetEventHandler(IMediator mediator, IInvoiceRepository invoiceRepository)
    : IntegrationEventHandler<AssetDto>
{
    private readonly IMediator _mediator = mediator;
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;

    protected override async Task HandleIntegrationEvent(ReceivedIntegrationEvent<AssetDto> notification, CancellationToken cancellationToken)
    {
        var asset = notification.Body!;
        var impactedInvoices = await _invoiceRepository.GetByAssetIdAsync(asset.Id);
        var services = impactedInvoices
            .SelectMany(i => i.GetServices())
            .Where(s => s.AssetId == asset.Id)
            .ToList();

        foreach (var service in services)
        {
            service.Name = asset.Name;
        }

        foreach (var invoice in impactedInvoices)
            await _mediator.Send(new UpdateEntityCommand<Invoice>(invoice.Id, invoice), cancellationToken);
    }
}
