using InvoiceAPI.Application.Features.Services;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class ServiceCRUDCommandsHandlerTests : BasicCRUDCommandsHandlerTests<ServiceCRUDCommandsHandler, ServiceDto, Service>
{
    protected override ServiceDto InstantiateApiEntity(Guid id)
    {
        return new ServiceDto
        {
            Id = id,
            Name = $"Service {id}",
            Price = 13.37M,
        };
    }

    protected override Service InstantiateDbEntity(Guid id)
    {
        return new Service
        {
            Id = id,
            Name = $"Service {id}",
            Price = 13.37M,
        };
    }
}
