using ErrorOr;

namespace InvoiceAPI.Application.Features.Invoices;

public static class InvoiceErrors
{
    public static readonly Error AssetsNotFound = Error.Validation
    ("AssetsNotFound", "Some referenced assets were not found in the Asset Service.");
}
