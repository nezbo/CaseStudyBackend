using AutoMapper;
using ErrorOr;
using InvoiceAPI.Application.External;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using MediatR;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.Features;

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
        var response = await _mediator.Send(new CreateEntityCommand<Invoice>(request.Data), cancellationToken);

        if (response.IsError)
            return response.Errors;

        var invoiceId = response.Value.Id;

        var assets = await _assetService.GetAssetsAsync(request.AssetIds.ToArray());
        var services = _mapper.Map<IEnumerable<Service>>(assets);
        await services
            .ForEach(s => s.InvoiceId = invoiceId)
            .ForEachAsync(s => _mediator.Send(new CreateEntityCommand<Service>(s), cancellationToken));

        return invoiceId;
    }
}
