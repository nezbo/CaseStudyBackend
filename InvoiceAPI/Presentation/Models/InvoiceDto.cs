using Microservice.Common.Presentation.Controllers;
using Microservice.Common.Presentation.Models;

namespace InvoiceAPI.Presentation.Models;

public record InvoiceDto : IIdentity, IHasEditUrl
{
    public required Guid Id { get; set; }
    public DateOnly IssuingDate { get; set; }
    public ushort Year { get; set; }
    public ushort Month { get; set; }
    public decimal Total { get; set; }
    public string? EditUrl { get; set; }

    public List<ServiceDto> Services { get; set; } = [];
}
