using ErrorOr;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public static class ServiceMapper
{
    public static ErrorOr<Service> ToDomain(this ServiceDto dto, Guid invoiceId)
    {
        return Service.Create(invoiceId, dto.AssetId, dto.Name, dto.Price, dto.Quantity);
    }

    public static ServiceDto ToDto(this Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            AssetId = service.AssetId,
            Name = service.Name,
            Price = service.Price,
            Quantity = service.Quantity
        };
    }
}
