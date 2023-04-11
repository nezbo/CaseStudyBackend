using Microservice.Common.Models;

namespace InvoiceAPI.Models.Database;

public record Service : BaseEntity
{
    public Guid InvoiceId { get; set; }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}