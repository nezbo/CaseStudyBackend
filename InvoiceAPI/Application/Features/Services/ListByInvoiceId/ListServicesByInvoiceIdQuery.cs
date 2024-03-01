using InvoiceAPI.Presentation.Models;
using MediatR;

namespace InvoiceAPI.Application.Features.Services.ListByInvoiceId;

public class ListServicesByInvoiceIdQuery : IRequest<IEnumerable<ServiceDto>>
{
    public Guid InvoiceId { get; set; }

    public ListServicesByInvoiceIdQuery(Guid invoiceId)
    {
        InvoiceId = invoiceId;
    }
}
