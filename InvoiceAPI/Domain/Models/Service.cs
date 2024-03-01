using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public record Service : BaseEntity
{
    public Guid InvoiceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}