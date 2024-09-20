using ErrorOr;

namespace InvoiceAPI.Domain.Errors;

public class ServiceErrors
{
    public static readonly Error PriceCanNotBeNegative = Error.Validation
    ("PriceCanNotBeNegative", "A Service can not have a negative price.");
    public static readonly Error ValidToMustBeAfterValidFrom = Error.Validation
    ("ValidToMustBeAfterValidFrom", "The ValidTo date must be after the ValidFrom date.");
}
