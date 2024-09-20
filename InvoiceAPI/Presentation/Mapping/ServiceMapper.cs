using ErrorOr;
using InvoiceAPI.Application.External.Models;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Presentation.Models;

namespace InvoiceAPI.Presentation.Mapping;

public static class ServiceMapper
{
    public static ErrorOr<Service> ToDomain(this ServiceDto dto)
    {
        return Service.Create(dto.InvoiceId, dto.Name, dto.Price, dto.ValidFrom, dto.ValidTo);
    }

    public static ServiceDto ToDto(this Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            InvoiceId = service.InvoiceId,
            Name = service.Name,
            Price = service.Price,
            ValidFrom = service.ValidFrom,
            ValidTo = service.ValidTo,
        };
    }
}
