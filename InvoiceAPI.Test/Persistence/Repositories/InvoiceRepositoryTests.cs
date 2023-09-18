using InvoiceAPI.Models.Database;
using InvoiceAPI.Persistence.Repositories;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Persistence.Repositories;

public class InvoiceRepositoryTests : GenericRepositoryTests<InvoiceRepository, Invoice>
{
    protected override Invoice InstantiateEntity(int entityNumber, Guid id)
    {
        return new Invoice
        {
            Id = id,
            IssuingDate = GetDate($"2020-01-{entityNumber:D2}"),
        };
    }

    private static DateOnly GetDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }
}