using InvoiceAPI.Models.Api;
using MediatR;

namespace InvoiceAPI.Features.Services.ListByInvoiceId;

public class ListServicesByInvoiceIdQuery : IRequest<IEnumerable<ServiceDto>>
{
    public Guid InvoiceId { get; set; }

    public ListServicesByInvoiceIdQuery(Guid invoiceId)
    {
        InvoiceId = invoiceId;
    }
}
