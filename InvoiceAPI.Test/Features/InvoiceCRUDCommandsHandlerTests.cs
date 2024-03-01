using InvoiceAPI.Application.Features.Invoices;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Features;

public class InvoiceCRUDCommandsHandlerTests : BasicCRUDCommandsHandlerTests<InvoiceCRUDCommandsHandler, InvoiceDto, Invoice>
{
    protected override InvoiceDto InstantiateApiEntity(Guid id)
    {
        return new InvoiceDto
        {
            Id = id,
            Month = (ushort)(id.GetHashCode() % 12 + 1),
        };
    }

    protected override Invoice InstantiateDbEntity(Guid id)
    {
        return new Invoice
        {
            Id = id,
            Month = (ushort)(id.GetHashCode() % 12 + 1),
        };
    }
}
