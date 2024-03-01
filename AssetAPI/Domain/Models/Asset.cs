using Microservice.Common.Domain.Models;

namespace AssetAPI.Domain.Models;

public record Asset : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
