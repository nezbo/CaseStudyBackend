using InvoiceAPI.Application.Features.Invoices;
using InvoiceAPI.Domain.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class InvoiceCRUDCommandsHandlerTests 
    : BasicCRUDCommandsHandlerTests<InvoiceCRUDCommandsHandler, Invoice>
{

    protected override Invoice InstantiateEntity(Guid id)
    {
        var month = (ushort)(id.GetHashCode() % 12 + 1);
        return new Invoice
        {
            Id = id,
            IssuingDate = new DateOnly(2024, month, 1),
            Year = 2024,
            Month = month,
            Total = 1337,
        };
    }
}
