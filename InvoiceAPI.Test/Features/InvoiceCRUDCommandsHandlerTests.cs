using InvoiceAPI.Application.Features.Invoices;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class InvoiceCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<InvoiceCRUDCommandsHandler, Invoice>
{

    protected override Invoice InstantiateEntity(Guid id)
    {
        return new Invoice(id)
        {
            Month = (ushort)(id.GetHashCode() % 12 + 1),
        };
    }
}
