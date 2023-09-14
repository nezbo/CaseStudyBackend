using AutoMapper;
using InvoiceAPI.External;
using InvoiceAPI.Models.Api;
using MediatR;
using Microservice.Common.CQRS;
using Microservice.Common.Extensions;

namespace InvoiceAPI.Features.Invoices.CreateByAssets;

public class CreateByAssetsHandler : IRequestHandler<CreateByAssetsCommand, Guid>
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

    public async Task<Guid> Handle(CreateByAssetsCommand request, CancellationToken cancellationToken)
    {
        var invoiceId = await _mediator.Send(new CreateEntityCommand<InvoiceDto>(request.Data), cancellationToken);

        var assets = await _assetService.GetAssetsAsync(request.AssetIds.ToArray());
        var services = _mapper.Map<IEnumerable<ServiceDto>>(assets);
        await services
            .ForEach(s => s.InvoiceId = invoiceId)
            .ForEachAsync(s => _mediator.Send(new CreateEntityCommand<ServiceDto>(s), cancellationToken));

        return invoiceId;
    }
}
