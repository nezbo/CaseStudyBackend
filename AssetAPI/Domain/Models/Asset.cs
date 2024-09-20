using Microservice.Common.Domain.Models;
using ErrorOr;
using AssetAPI.Domain.Errors;

namespace AssetAPI.Domain.Models;

public class Asset : AggregateRoot
{
    public Asset() : base(null) { }
    private Asset(Guid? id) : base(id) { }

    public static ErrorOr<Asset> Create(Guid? id, string name, decimal price, DateOnly? validFrom, DateOnly? validTo)
    {
        if (validFrom.HasValue && validTo.HasValue
            && validTo < validFrom)
            return AssetErrors.ValidToMustBeAfterValidFrom;
        if (price < 0)
            return AssetErrors.PriceCanNotBeNegative;

        return new Asset(id)
        {
            Name = name,
            Price = price,
            ValidFrom = validFrom,
            ValidTo = validTo,
        };
    }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }

    /*public ErrorOr<Success> SetPrice(decimal price)
    {
        if (price < 0)
            return AssetErrors.PriceCanNotBeNegative;

        Price = price;
        return Result.Success;
    }*/
}
