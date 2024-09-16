using AutoMapper;
using ErrorOr;
using InvoiceAPI.Application.External;
using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.Features;
using MoreLinq;

namespace InvoiceAPI.Application.Features.Invoices.CreateByAssets;

public class CreateByAssetsHandler 
    : IRequestHandler<CreateByAssetsCommand, ErrorOr<Guid>>
{
    private readonly IMediator _mediator;
    private readonly IAssetService _assetService;
    private readonly IMapper _mapper;

    public CreateByAssetsHandler(IMediator mediator, IAssetService assetService, IMapper mapper)
    {
        _mediator = mediator;
        _assetService = assetService;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateByAssetsCommand request, CancellationToken cancellationToken)
    {
        var assets = await _assetService.GetAssetsAsync(request.AssetIds.ToArray());
        foreach (var asset in assets)
        {
            var service = MapAssetToService(asset, request.Data.Id);

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

    private static ErrorOr<Service> MapAssetToService(AssetDto asset, Guid invoiceId)
    {
        return Service.Create(invoiceId, asset.Name, asset.Price, asset.ValidFrom, asset.ValidTo);
    }
}
