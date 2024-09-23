using ErrorOr;
using InvoiceAPI.Domain.Errors;
using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Service : Entity
{
    public Guid InvoiceId { get; private set; }
    public Guid AssetId { get; private set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateOnly? ValidFrom { get; private set; }
    public DateOnly? ValidTo { get; private set; }

    public Service() : this(null) { }
    private Service(Guid? id) : base(id) { }

    public static ErrorOr<Service> Create(
        Guid invoiceId,
        Guid assetId,
        string name, 
        decimal price, 
        DateOnly? validFrom, 
        DateOnly? validTo)
    {
        if (validFrom.HasValue && validTo.HasValue
            && validTo < validFrom)
            return ServiceErrors.ValidToMustBeAfterValidFrom;
        if (price < 0)
            return ServiceErrors.PriceCanNotBeNegative;

        return new Service
        {
            InvoiceId = invoiceId,
            AssetId = assetId,
            Name = name,
            Price = price,
            ValidFrom = validFrom,
            ValidTo = validTo
        };
    }
}