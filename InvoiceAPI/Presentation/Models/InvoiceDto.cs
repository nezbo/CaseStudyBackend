using Microservice.Common.Domain.Models;
using Microservice.Common.Presentation.Controllers;

namespace InvoiceAPI.Presentation.Models;

public record InvoiceDto : IIdentity, IHasEditUrl
{
    public Guid Id { get; set; }
    public DateOnly IssuingDate { get; set; }
    public ushort Year { get; set; }
    public ushort Month { get; set; }
    public decimal Total { get; set; }
    public string? EditUrl { get; set; }
}
