using InvoiceAPI.Presentation.Models;
using MediatR;

namespace InvoiceAPI.Application.Features.Invoices.CreateByAssets;

public class CreateByAssetsCommand : IRequest<Guid>
{
    public required InvoiceDto Data { get; set; }
    public required IEnumerable<Guid> AssetIds { get; set; }
}
