using ErrorOr;
using InvoiceAPI.Domain.Models;
using MediatR;

namespace InvoiceAPI.Application.Features.Invoices.CreateByAssets;

public class CreateByAssetsCommand : IRequest<ErrorOr<Guid>>
{
    public required Invoice Data { get; set; }
    public required IEnumerable<Guid> AssetIds { get; set; }
}
