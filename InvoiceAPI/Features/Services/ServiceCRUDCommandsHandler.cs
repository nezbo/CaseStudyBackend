using AutoMapper;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;
using Microservice.Common.Features;
using Microservice.Common.Repository;

namespace InvoiceAPI.Features.Services;

public class ServiceCRUDCommandsHandler : BasicCRUDCommandsHandler<ServiceDto, Service>
{
    public ServiceCRUDCommandsHandler(IGenericUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}
