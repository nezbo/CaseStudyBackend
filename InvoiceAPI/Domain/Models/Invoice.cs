using ErrorOr;
using InvoiceAPI.Domain.Errors;
using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Invoice : AggregateRoot
{
    public Invoice() : base(null) { }
    private Invoice(Guid? id) : base(id) { }

    public static ErrorOr<Invoice> Create(
        Guid? id,
        DateOnly issuingDate,
        ushort year,
        ushort month,
        decimal total)
    {
        if (total < 0)
            return InvoiceErrors.AmountCanNotBeNegative;

        return new Invoice(id)
        {
            IssuingDate = issuingDate,
            Year = year,
            Month = month,
            Total = total
        };
    }

    public required DateOnly IssuingDate { get; set; }
    public required ushort Year { get; set; }
    public required ushort Month { get; set; }
    public decimal Total { get; private set; }

    private readonly List<Service> _services = [];

    public IEnumerable<Service> GetServices() => _services.AsReadOnly();

    public ErrorOr<Success> AddService(Service service)
    {
        if (service.InvoiceId != Id)
            return InvoiceErrors.ServiceMustReferenceInvoice;

        _services.Add(service);
        return Result.Success;
    }
}
