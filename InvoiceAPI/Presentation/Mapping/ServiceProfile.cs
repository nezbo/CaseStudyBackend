using AutoMapper;
using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(i => i.Services, c => c.MapFrom(i => i.GetServices()));
        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>()
            .ConstructUsing(dto => Service.Create(dto.InvoiceId, dto.Name, dto.Price, dto.ValidFrom, dto.ValidTo).Value);

        CreateMap<AssetDto, ServiceDto>();
    }
}
