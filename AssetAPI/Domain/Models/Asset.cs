using Microservice.Common.Domain.Models;
using ErrorOr;
using AssetAPI.Domain.Errors;

namespace AssetAPI.Domain.Models;

public class Asset(Guid? id) : AggregateRoot(id)
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }

    public ErrorOr<Success> SetPrice(decimal price)
    {
        if (price < 0)
            return AssetErrors.AmountCanNotBeNegative;

        Price = price;
        return Result.Success;
    }

    public Asset() : this(null) { }
}
