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
}