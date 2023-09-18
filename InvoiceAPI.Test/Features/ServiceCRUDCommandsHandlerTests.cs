using InvoiceAPI.Features.Services;
using InvoiceAPI.Models.Api;
using InvoiceAPI.Models.Database;
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
        };
    }

    protected override Service InstantiateDbEntity(Guid id)
    {
        return new Service
        {
            Id = id,
            Name = $"Service {id}",
        };
    }
}
