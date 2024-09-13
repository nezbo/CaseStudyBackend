using InvoiceAPI.Application.Features.Services;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class ServiceCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<ServiceCRUDCommandsHandler, Service>
{
    protected override Service InstantiateEntity(Guid id)
    {
        return new Service(id)
        {
            Name = $"Service {id}",
            Price = 13.37M,
        };
    }
}
