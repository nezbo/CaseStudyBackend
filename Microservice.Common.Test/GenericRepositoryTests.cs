using FluentAssertions;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Models;
using Microservice.Common.Repository;
using Microservice.Common.Test.Core;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Microservice.Common.Test;

public abstract class GenericRepositoryTests<TRepository,TEntity> : BaseTestFixture<TRepository> 
    where TRepository : class, IGenericRepository<TEntity>
    where TEntity : class, IIdentity
{
    protected abstract TEntity InstantiateEntity(int entityNumber, Guid id);

    protected virtual IEnumerable<TEntity> SetupDbContextSet(int count)
    {
        var entities = Enumerable.Range(1, count)
            .Select(i =>
            {
                var id = GetGuid(i);
                var e = InstantiateEntity(i, id);
                e.Id = id;
                return e;
            })
            .ToList();

        this.SetupDbContextSet(entities);

        return entities;
    }

    protected virtual void SetupDbContextSet(IEnumerable<TEntity> entities)
    {
        var mock = entities.AsQueryable().BuildMockDbSet();
        mock.Setup(x => x.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((object[] ids) =>
            {
                var id = (Guid)ids[0];
                return entities.FirstOrDefault(x => x.Id == id);
            });
        Container.Use(mock);

        Container.GetMock<IBaseDbContext>()
            .Setup(x => x.GetSet<TEntity>())
            .Returns(() => Container.Get<DbSet<TEntity>>());
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

        (result != null).Should().Be(expectsMatch);
        result?.Id.Should().Be(id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(9)]
    public async Task GetAllAsync_Should_Retrieve_All(int expectedCount)
    {
        SetupDbContextSet(expectedCount);

        var result = await Sut.GetAllAsync();

        result.Should().HaveCount(expectedCount);
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

        Container.Verify<DbSet<TEntity>>(s =>
                s.AddAsync(It.Is<TEntity>(e => e == newEntity), It.IsAny<CancellationToken>()).Result,
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_DbSet_Update()
    {
        var entities = SetupDbContextSet(2);
        var updatedEntity = entities.First();

        await Sut.UpdateAsync(updatedEntity);

        Container.Verify<DbSet<TEntity>>(s =>
            s.Update(It.Is<TEntity>(e => e == updatedEntity)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Call_DbSet_When_Null()
    {
        var entities = SetupDbContextSet(2);
        TEntity updatedEntity = null;

        await Sut.UpdateAsync(updatedEntity);

        Container.Verify<DbSet<TEntity>>(s =>
            s.Update(It.Is<TEntity>(e => e == updatedEntity)), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public async Task DeleteAsync_Should_Handle_Entities_Correctly(int idNumber)
    {
        Guid id = GetGuid(idNumber);
        SetupDbContextSet(2);
        
        await Sut.DeleteAsync(id);

        Container.Verify<DbSet<TEntity>>(s =>
            s.Remove(It.Is<TEntity>(e => e.Id == id)), Times.Once);
    }

    private Guid GetGuid(int digit)
    {
        char c = digit.ToString()[0];
        return new Guid($"{GetTimes(c,8)}-{GetTimes(c, 4)}-{GetTimes(c, 4)}-{GetTimes(c, 4)}-{GetTimes(c, 12)}");
    }

    private string GetTimes(char digit, int count)
    {
        return new string(digit, count);
    }


}
