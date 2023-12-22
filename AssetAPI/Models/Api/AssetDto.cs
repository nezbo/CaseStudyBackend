using Microservice.Common.Controllers;
using Microservice.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetAPI.Models.Api;

public record AssetDto : IIdentity, IHasEditUrl
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public string? EditUrl { get; set; }
}
