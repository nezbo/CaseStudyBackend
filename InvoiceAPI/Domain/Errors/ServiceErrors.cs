using ErrorOr;

namespace InvoiceAPI.Domain.Errors;

public class ServiceErrors
{
    public static readonly Error ValidToMustBeAfterValidFrom = Error.Validation
    ("ValidToMustBeAfterValidFrom", "The ValidTo date must be after the ValidFrom date.");
}
