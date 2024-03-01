using AutoMapper;
using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>();

        CreateMap<AssetDto, ServiceDto>();
    }
}
