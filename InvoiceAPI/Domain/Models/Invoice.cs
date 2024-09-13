using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Invoice(Guid? id) : Entity(id)
{
    public DateOnly IssuingDate { get; set; }
    public ushort Year { get; set; }
    public ushort Month { get; set; }
    public decimal Total { get; set; }

    public Invoice() : this(null) { }
}
