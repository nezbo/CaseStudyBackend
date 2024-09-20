using InvoiceAPI.Domain.Models;
using InvoiceAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Persistence.Repository;

public class InvoiceRepositoryTests : GenericRepositoryTests<InvoiceRepository, Invoice>
{
    protected override Invoice InstantiateEntity(int entityNumber, Guid id)
    {
        var date = GetDate($"2020-01-{entityNumber:D2}");
        return Invoice.Create(id, date, (ushort)date.Year, (ushort)date.Month, 100).Value;
    }

    private static DateOnly GetDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }
}