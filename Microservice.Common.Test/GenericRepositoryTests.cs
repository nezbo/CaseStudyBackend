using FluentAssertions;
using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Test.Core;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Microservice.Common.Test;

public abstract class GenericRepositoryTests<TRepository,TEntity> : BaseTestFixture<TRepository> 
    where TRepository : class, IGenericRepository<TEntity>
    where TEntity : Entity
{
    protected abstract TEntity InstantiateEntity(int entityNumber, Guid id);

    protected virtual IEnumerable<TEntity> SetupDbContextSet(int count)
    {
        var entities = Enumerable.Range(1, count)
            .Select(i =>
            {
                var id = GetGuid(i);
                var e = InstantiateEntity(i, id);
                return e;
            })
            .ToList();

        this.SetupDbContextSet(entities);

        return entities;
    }

    protected virtual void SetupDbContextSet(IEnumerable<TEntity> entities)
    {
        var mock = BuildMockDbSet(entities);
        mock.FindAsync(Arg.Any<object[]>())
            .Returns(x =>
            {
                var id = (Guid)x.Arg<object[]>()[0];
                return entities.FirstOrDefault(x => x.Id == id);
            });
        Builder.Provide(mock);

        Container.Resolve<IBaseDbContext>()
            .GetSet<TEntity>()
            .Returns(x => Container.Resolve<DbSet<TEntity>>());
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(3, false)]
    [InlineData(0, false)]
    public async Task GetByIdAsync_Should_Return_Expected(int idNumber, bool expectsMatch)
    {
        SetupDbContextSet(2);

        Guid id = GetGuid(idNumber);
        var result = await Sut.GetByIdAsync(id);

        result.IsError.Should().Be(!expectsMatch);
        result.Value?.Id.Should().Be(id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(9)]
    public async Task GetAllAsync_Should_Retrieve_All(int expectedCount)
    {
        SetupDbContextSet(expectedCount);

        var result = await Sut.GetAllAsync();

        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task AddAsync_Should_Call_DbSet_Add(int existingEntities)
    {
        SetupDbContextSet(existingEntities);

        var newEntity = this.InstantiateEntity(3, GetGuid(3));

        await Sut.AddAsync(newEntity);

        await Container.Resolve<DbSet<TEntity>>()
            .Received(1)
            .AddAsync(Arg.Is<TEntity>(e => e == newEntity), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_DbSet_Update()
    {
        var entities = SetupDbContextSet(2);
        var updatedEntity = entities.First();

        await Sut.UpdateAsync(updatedEntity);

        Container.Resolve<DbSet<TEntity>>()
            .Received(1)
            .Update(Arg.Is<TEntity>(e => e == updatedEntity));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(3, false)]
    public async Task DeleteAsync_Should_Handle_Entities_Correctly(int idNumber, bool shouldCallRemove)
    {
        Guid id = GetGuid(idNumber);
        SetupDbContextSet(2);
        
        await Sut.DeleteAsync(id);

        Container.Resolve<DbSet<TEntity>>()
            .Received(shouldCallRemove ? 1 : 0)
            .Remove(Arg.Is<TEntity>(e => e.Id == id));
    }

    private static DbSet<TEntity> BuildMockDbSet(IEnumerable<TEntity> entities)
    {
        var data = entities.AsQueryable();
        var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>>();

        // And then as you do:
        ((IQueryable<TEntity>)mockSet).Provider.Returns(data.Provider);
        ((IQueryable<TEntity>)mockSet).Expression.Returns(data.Expression);
        ((IQueryable<TEntity>)mockSet).ElementType.Returns(data.ElementType);
        ((IQueryable<TEntity>)mockSet).GetEnumerator().Returns(data.GetEnumerator());

        return mockSet;
    }

    private static Guid GetGuid(int digit)
    {
        char c = digit.ToString()[0];
        return new Guid($"{GetTimes(c,8)}-{GetTimes(c, 4)}-{GetTimes(c, 4)}-{GetTimes(c, 4)}-{GetTimes(c, 12)}");
    }

    private static string GetTimes(char digit, int count)
    {
        return new string(digit, count);
    }


}
