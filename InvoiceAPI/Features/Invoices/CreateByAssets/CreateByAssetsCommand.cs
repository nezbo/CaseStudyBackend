using InvoiceAPI.Models.Api;
using MediatR;

namespace InvoiceAPI.Features.Invoices.CreateByAssets;

public class CreateByAssetsCommand : IRequest<Guid>
{
    public required InvoiceDto Data { get; set; }
    public required IEnumerable<Guid> AssetIds { get; set; }
}
