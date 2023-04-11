using Microservice.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetAPI.Models.Api;

public record AssetDto : IIdentity
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
