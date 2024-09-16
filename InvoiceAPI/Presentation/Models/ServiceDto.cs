using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Presentation.Models;

public record ServiceDto : IIdentity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }

    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
