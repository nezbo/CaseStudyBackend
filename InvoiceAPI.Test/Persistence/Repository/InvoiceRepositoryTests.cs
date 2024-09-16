using InvoiceAPI.Domain.Models;
using InvoiceAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Persistence.Repository;

public class InvoiceRepositoryTests : GenericRepositoryTests<InvoiceRepository, Invoice>
{
    protected override Invoice InstantiateEntity(int entityNumber, Guid id)
    {
        var date = GetDate($"2020-01-{entityNumber:D2}");
        return new Invoice
        {
            Id = id,
            Year = (ushort)date.Year,
            Month = (ushort)date.Month,
            IssuingDate = date,
            Total = 100,
        };
    }

    private static DateOnly GetDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }
}