using FluentAssertions;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Test;

namespace InvoiceAPI.Test.Persistence.Repository;

public class ServiceRepositoryTests : GenericRepositoryTests<ServiceRepository, Service>
{
    protected override Service InstantiateEntity(int entityNumber, Guid id)
    {
        return new Service(id)
        {
            ValidFrom = entityNumber >= 2 ? GetDate("2020-01-01") : null,
            ValidTo = entityNumber <= 2 ? GetDate("2022-01-01") : null,
        };
    }

    private DateOnly? GetDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }

    [Fact]
    public async Task GetByInvoiceAsync_Should_Return_References()
    {
        var entities = SetupDbContextSet(3).ToList();
        var invoiceId = Guid.NewGuid();
        entities[0].InvoiceId = invoiceId;
        entities[1].InvoiceId = invoiceId;
        entities[2].InvoiceId = Guid.NewGuid();

        var result = await Sut.GetByInvoiceIdAsync(invoiceId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByInvoiceAsync_Should_Return_Empty_When_None_Found()
    {
        var entities = SetupDbContextSet(3).ToList();
        var invoiceId = Guid.NewGuid();
        entities[0].InvoiceId = Guid.NewGuid();
        entities[1].InvoiceId = Guid.NewGuid();
        entities[2].InvoiceId = Guid.NewGuid();

        var result = await Sut.GetByInvoiceIdAsync(invoiceId);

        result.Should().BeEmpty();
    }


}
