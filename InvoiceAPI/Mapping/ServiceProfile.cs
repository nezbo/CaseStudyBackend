using AutoMapper;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;

namespace InvoiceAPI.Mapping;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>();
    }
}
