using AutoMapper;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;

namespace InvoiceAPI.Mapping;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceDto, Invoice>();
    }
}
