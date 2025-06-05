using ErrorOr;
using InvoiceAPI.Domain.Errors;
using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Service : AggregateRoot
{
    public Guid InvoiceId { get; private set; }
    public Invoice? Invoice { get; private set; }
    public Guid AssetId { get; private set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Quantity { get; set; } = 1;

    public Service() : this(null) { }
    private Service(Guid? id) : base(id) { }

    public static ErrorOr<Service> Create(
        Guid invoiceId,
        Guid assetId,
        string name, 
        decimal price,
        int quantity)
    {
        if (quantity < 1)
            return ServiceErrors.QuantityMustBePositive;
        if (price < 0)
            return ServiceErrors.PriceCanNotBeNegative;

        return new Service
        {
            InvoiceId = invoiceId,
            AssetId = assetId,
            Name = name,
            Price = price,
            Quantity = quantity
        };
    }
}