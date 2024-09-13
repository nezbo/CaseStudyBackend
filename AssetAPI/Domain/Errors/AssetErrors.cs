using ErrorOr;

namespace AssetAPI.Domain.Errors;

public static class AssetErrors
{
    public static readonly Error AmountCanNotBeNegative = Error.Validation
        ("PriceCanNotBeNegative", "An Asset can not have a negative price.");
}
