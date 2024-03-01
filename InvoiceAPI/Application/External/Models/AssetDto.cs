using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Application.External.Models;

public record AssetDto : IIdentity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
