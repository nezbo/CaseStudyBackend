using InvoiceAPI.Application.Features.Invoices;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class InvoiceCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<InvoiceCRUDCommandsHandler, Invoice>
{

    protected override Invoice InstantiateEntity(Guid id)
    {
        ushort month = Convert.ToUInt16((Math.Abs(id.GetHashCode()) % 12) + 1);
        return Invoice.Create(id, new DateOnly(2024, month, 1), 2024, month, 1337).Value;
    }
}