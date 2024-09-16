using ErrorOr;
using InvoiceAPI.Domain.Errors;
using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Service(Guid? id) : Entity(id)
{
    public Guid InvoiceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }

    public Service() : this(null) { }

    public static ErrorOr<Service> Create(Guid invoiceId, 
        string name, 
        decimal price, 
        DateOnly? validFrom, 
        DateOnly? validTo)
    {
        if (validFrom.HasValue && validTo.HasValue
            && validTo < validFrom)
            return ServiceErrors.ValidToMustBeAfterValidFrom;

        return new Service
        {
            InvoiceId = invoiceId,
            Name = name,
            Price = price,
            ValidFrom = validFrom,
            ValidTo = validTo
        };
    }
}