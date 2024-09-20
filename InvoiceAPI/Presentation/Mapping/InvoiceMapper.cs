using ErrorOr;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public static class InvoiceMapper
{
    public static ErrorOr<Invoice> ToDomain(this InvoiceDto dto)
    {
        var result = Invoice.Create(dto.IssuingDate, dto.Year, dto.Month, dto.Total);
        IEnumerable<ErrorOr<Service>> services = dto.Services.Select(s => s.ToDomain());

        if (result.IsError || services.Any(s => s.IsError))
            return result.Errors
                .Union(services.SelectMany(s => s.Errors))
                .ToList();

        return result;
    }

    public static InvoiceDto ToDto(this Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            IssuingDate = invoice.IssuingDate,
            Year = invoice.Year,
            Month = invoice.Month,
            Total = invoice.Total,
            Services = invoice.GetServices().Select(s => s.ToDto()).ToList(),
        };
    }
}
