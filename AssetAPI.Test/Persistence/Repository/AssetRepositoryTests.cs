using AssetAPI.Domain.Models;
using AssetAPI.Infrastructure.Persistence.Repository;
using FluentAssertions;
using Microservice.Common.Test;

namespace AssetAPI.Test.Persistence.Repository;

public class AssetRepositoryTests : GenericRepositoryTests<AssetRepository, Asset>
{
    protected override Asset InstantiateEntity(int entityNumber, Guid id)
        => Asset.Create(id,
                        entityNumber.ToString(),
                        0M,
                        entityNumber >= 2 ? GetDate("2020-01-01") : null,
                        entityNumber <= 2 ? GetDate("2022-01-01") : null).Value;

    [Theory]
    [InlineData("2021-01-01", 3)]
    [InlineData("1999-01-01", 1)]
    [InlineData("2030-01-01", 1)]
    public async Task GetValidOnAsync_Should_Handle_Ranges_Correctly(string testValidOn, int expectedCount)
    {
        SetupDbContextSet(3);

        var response = await Sut.GetValidOnAsync(GetDate(testValidOn));

        response.IsError.Should().BeFalse();
        response.Value.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData("2021-01-01", 1)]
    [InlineData("1999-01-01", 1)]
    [InlineData("2030-01-01", 1)]
    public async Task GetValidOnAsync_Should_Return_Entity_When_Always_Valid(string testValidOn, int expectedCount)
    {
        var entities = SetupDbContextSet(1);
        entities.Single().ValidTo = null;
        entities.Single().ValidFrom = null;

        var response = await Sut.GetValidOnAsync(GetDate(testValidOn));

        response.IsError.Should().BeFalse();
        response.Value.Should().HaveCount(expectedCount);
    }

    private DateOnly GetDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }
}
