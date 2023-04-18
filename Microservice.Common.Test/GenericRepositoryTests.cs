using FluentAssertions;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Extensions;
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
    protected abstract TEntity InstantiateEntity(Guid id);

    protected void SetupDbContextSet(int count)
    {
        var entities = Enumerable.Range(1, count)
            .Select(i => 
            { 
                var id = GetGuid(i); 
                var e = InstantiateEntity(id); 
                e.Id = id; 
                return e; 
            });

        this.SetupDbContextSet(entities);
    }

    protected void SetupDbContextSet(IEnumerable<TEntity> entities)
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
    public async Task GetById_Should_Return_Expected(int idNumber, bool expectsMatch)
    {
        SetupDbContextSet(2);

        Guid id = GetGuid(idNumber);
        var result = await Sut.GetByIdAsync(id);

        (result != null).Should().Be(expectsMatch);
        result?.Id.Should().Be(id);
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
