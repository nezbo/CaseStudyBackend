using AutoMapper;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceDto, Invoice>()
            .ConstructUsing(MapInvoice);
    }

    private Invoice MapInvoice(InvoiceDto dto, ResolutionContext context)
    {
        return Invoice.Create(dto.IssuingDate, dto.Year, dto.Month, dto.Total).Value;
    }
}
