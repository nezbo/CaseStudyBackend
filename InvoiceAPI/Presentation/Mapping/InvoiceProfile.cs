using AutoMapper;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceDto, Invoice>();
    }
}
