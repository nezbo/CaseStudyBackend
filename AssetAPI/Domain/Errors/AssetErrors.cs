using ErrorOr;

namespace AssetAPI.Domain.Errors;

public static class AssetErrors
{
    public static readonly Error PriceCanNotBeNegative = Error.Validation
        ("PriceCanNotBeNegative", "An Asset can not have a negative price.");
    public static readonly Error ValidToMustBeAfterValidFrom = Error.Validation
    ("ValidToMustBeAfterValidFrom", "The ValidTo date must be after the ValidFrom date.");
}
