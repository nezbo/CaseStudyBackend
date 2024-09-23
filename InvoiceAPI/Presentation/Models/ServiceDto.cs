using Microservice.Common.Presentation.Models;

namespace InvoiceAPI.Presentation.Models;

public record ServiceDto : IIdentity
{
    public required Guid Id { get; set; }
    public required Guid AssetId { get; set; }

    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
