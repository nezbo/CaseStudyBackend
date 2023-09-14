using InvoiceAPI.Models.Api;
using MediatR;

namespace InvoiceAPI.Features.Invoices.CreateByAssets;

public class CreateByAssetsCommand : IRequest<Guid>
{
    public InvoiceDto Data { get; set; }
    public IEnumerable<Guid> AssetIds { get; set; }
}
