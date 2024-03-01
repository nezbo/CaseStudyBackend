using Microservice.Common.Domain.Models;
using Microservice.Common.Presentation.Controllers;

namespace AssetAPI.Presentation.Models;

public record AssetDto : IIdentity, IHasEditUrl
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public string? EditUrl { get; set; }
}
