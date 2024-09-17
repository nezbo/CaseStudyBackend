using ErrorOr;

namespace InvoiceAPI.Domain.Errors;

public static class InvoiceErrors
{
    public static readonly Error AmountCanNotBeNegative = Error.Validation
    ("PriceCanNotBeNegative", "An Invoice can not have a negative price.");
    public static readonly Error AssetsNotFound = Error.Validation
    ("AssetsNotFound", "Some referenced assets were not found in the Asset Service.");
}
