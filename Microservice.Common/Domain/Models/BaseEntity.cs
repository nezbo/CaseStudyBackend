using System.ComponentModel.DataAnnotations;

namespace Microservice.Common.Domain.Models;

public record BaseEntity : IIdentity
{
    [Key]
    public Guid Id { get; set; }
}
