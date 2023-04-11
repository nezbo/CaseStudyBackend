using Microservice.Common.Models;

namespace InvoiceAPI.Models.Api;

public record InvoiceDto : IIdentity
{
    public Guid Id { get; set; }
    public DateOnly IssuingDate { get; set; }
    public ushort Year { get; set; }
    public ushort Month { get; set; }
    public decimal Total { get; set; }
}
