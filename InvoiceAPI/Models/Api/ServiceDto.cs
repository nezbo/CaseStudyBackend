using Microservice.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAPI.Models.Api;

public record ServiceDto : IIdentity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
