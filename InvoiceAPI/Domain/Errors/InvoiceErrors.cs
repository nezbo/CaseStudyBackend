using ErrorOr;

namespace InvoiceAPI.Domain.Errors;

public static class InvoiceErrors
{
    public static readonly Error AmountCanNotBeNegative = Error.Validation
    ("PriceCanNotBeNegative", "An Invoice can not have a negative price.");
    public static readonly Error ServiceMustReferenceInvoice = Error.Validation
    ("ServiceMustReferenceInvoice", "The Services on an Invoice must reference it.");
}
