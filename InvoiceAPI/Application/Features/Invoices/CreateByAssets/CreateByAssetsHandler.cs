using ErrorOr;
using InvoiceAPI.Application.External;
using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Domain.Models;
using MediatR;
using Microservice.Common.Application.Features;

namespace InvoiceAPI.Application.Features.Invoices.CreateByAssets;

public class CreateByAssetsHandler(
    IMediator mediator,
    IAssetService assetService)
        : IRequestHandler<CreateByAssetsCommand, ErrorOr<Guid>>
{
    private readonly IMediator _mediator = mediator;
    private readonly IAssetService _assetService = assetService;

    public async Task<ErrorOr<Guid>> Handle(CreateByAssetsCommand request, CancellationToken cancellationToken)
    {
        var assets = await _assetService.GetAssetsAsync(request.AssetIds);

        if (!AllAssetsFound(request, assets))
            return InvoiceErrors.AssetsNotFound;

        foreach (var assetId in assets.Select(a => a.Id).Distinct())
        {
            var quantity = assets.Count(a => a.Id == assetId);
            var asset = assets.First(a => a.Id == assetId);

            var service = MapAssetToService(asset, request.Data.Id, quantity);

            if (service.IsError)
                return service.Errors;

            var success = request.Data.AddService(service.Value);
            if (success.IsError)
                return success.Errors;
        }

        var response = await _mediator.Send(new CreateEntityCommand<Invoice>(request.Data), cancellationToken);

        if (response.IsError)
            return response.Errors;

        return response.Value.Id;
    }

    private static bool AllAssetsFound(CreateByAssetsCommand request, IEnumerable<AssetDto> assets)
    {
        return assets.Select(a => a.Id).Order().SequenceEqual(request.AssetIds.Order());
    }

    private static ErrorOr<Service> MapAssetToService(AssetDto asset, Guid invoiceId, int quantity = 1)
    {
        return Service.Create(invoiceId, asset.Id, asset.Name, asset.Price, quantity);
    }
}
