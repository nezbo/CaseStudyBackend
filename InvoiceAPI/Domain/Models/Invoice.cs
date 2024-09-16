using ErrorOr;
using InvoiceAPI.Domain.Errors;
using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public class Invoice : AggregateRoot
{
    public static ErrorOr<Invoice> Create(DateOnly issuingDate, ushort year, ushort month, decimal total)
    {
        if (total < 0)
            return InvoiceErrors.AmountCanNotBeNegative;

        return new Invoice
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
    public required decimal Total { get; set; }

    private readonly List<Service> _services = [];

    public Invoice() : base(null) { }

    public IEnumerable<Service> GetServices() => _services.AsReadOnly();

    public ErrorOr<Success> AddService(Service service)
    {
        _services.Add(service);
        return Result.Success;
    }
}
