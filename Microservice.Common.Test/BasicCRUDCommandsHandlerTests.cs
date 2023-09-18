using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microservice.Common.CQRS;
using Microservice.Common.Features;
using Microservice.Common.Models;
using Microservice.Common.Repository;
using Microservice.Common.Test.Core;
using Moq;

namespace Microservice.Common.Test;

public abstract class BasicCRUDCommandsHandlerTests<THandler, TApi, TDatabase> : BaseTestFixture<THandler>
    where THandler : BasicCRUDCommandsHandler<TApi, TDatabase>
    where TApi : IIdentity
    where TDatabase : class, IIdentity
{
    public BasicCRUDCommandsHandlerTests()
    {
        // IGenericUnitOfWork
        Container.GetMock<IGenericUnitOfWork>()
            .Setup(u => u.GetRepository<TDatabase>())
            .Returns(() => Container.GetMock<IGenericRepository<TDatabase>>().Object);

        // IGenericRepository
        Container.GetMock<IGenericRepository<TDatabase>>()
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .Returns<Guid>(id => Task.FromResult(InstantiateDbEntity(id)));

        // IMapper
        Container.GetMock<IMapper>()
            .Setup(m => m.Map<TDatabase>(It.IsAny<TApi>()))
            .Returns<TApi>(o => InstantiateDbEntity(o.Id));
        Container.GetMock<IMapper>()
            .Setup(m => m.Map<TApi>(It.IsAny<TDatabase>()))
            .Returns<TDatabase>(o => InstantiateApiEntity(o.Id));
    }

    protected abstract TApi InstantiateApiEntity(Guid id);
    protected abstract TDatabase InstantiateDbEntity(Guid id);

    [Fact]
    public async Task Handle_CreateEntityCommand_ShouldCreateEntity()
    {
        // Arrange
        var entity = InstantiateApiEntity(Guid.NewGuid());
        var command = new CreateEntityCommand<TApi>(entity);

        // Act
        Guid id = await Sut.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBeEmpty();

        Container.Verify<IGenericRepository<TDatabase>>(r => r.AddAsync(It.Is<TDatabase>(d => d.Id == id)), Times.Once);
        Container.Verify<IGenericUnitOfWork>(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GetEntityQuery_ShouldReturnEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        TApi entity = await Sut.Handle(new GetEntityQuery<TApi>(id), CancellationToken.None);

        // Assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(id);

        Container.Verify<IGenericRepository<TDatabase>>(r => r.GetByIdAsync(id), Times.Once);
        Container.Verify<IGenericRepository<TDatabase>>(r => r.GetByIdAsync(It.Is<Guid>(i => i != id)), Times.Never);
    }

    [Fact]
    public async Task Handle_ListAllEntitiesQuery_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = new[]
        {
            InstantiateDbEntity(Guid.NewGuid()),
            InstantiateDbEntity(Guid.NewGuid()),
        };
        Container.GetMock<IGenericRepository<TDatabase>>()
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(entities);

        // Act
        IEnumerable<TApi> result = await Sut.Handle(new ListAllEntitiesQuery<TApi>(), CancellationToken.None);

        // Assert
        entities.Should().NotBeEmpty();
        entities.Should().HaveCount(2);

        Container.Verify<IGenericRepository<TDatabase>>(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ListEntitiesQuery_ShouldReturnEntities()
    {
        // Arrange
        var entities = new[]
        {
            InstantiateDbEntity(Guid.NewGuid()),
            InstantiateDbEntity(Guid.NewGuid()),
        };
        Container.GetMock<IGenericRepository<TDatabase>>()
            .Setup(r => r.GetByIdsAsync(It.IsAny<Guid[]>()))
            .Returns<Guid[]>(ids => Task.FromResult(entities.Where(e => ids.Contains(e.Id))));

        // Act
        var query = entities.Select(e => e.Id).Append(Guid.NewGuid()).ToArray();
        IEnumerable<TApi> result = await Sut.Handle(new ListEntitiesQuery<TApi>(query), CancellationToken.None);

        // Assert
        entities.Should().NotBeEmpty();
        entities.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ListEntitiesQuery_ShouldThrowWhenNoInds()
    {
        var action = () => Sut.Handle(new ListEntitiesQuery<TApi>(Enumerable.Empty<Guid>()), CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_UpdateEntityCommand_ShouldUpdateEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var updateEntity = InstantiateApiEntity(id);
        var command = new UpdateEntityCommand<TApi>(updateEntity);

        // Act
        await Sut.Handle(command, CancellationToken.None);

        // Assert
        Container.Verify<IGenericRepository<TDatabase>>(r => r.UpdateAsync(It.Is<TDatabase>(d => d.Id == id)), Times.Once);
        Container.Verify<IGenericUnitOfWork>(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteEntityCommand_ShouldDeleteEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var command = new DeleteEntityCommand<TApi>(id);

        // Act
        await Sut.Handle(command, CancellationToken.None);

        // Assert
        Container.Verify<IGenericRepository<TDatabase>>(r => r.DeleteAsync(id), Times.Once);
        Container.Verify<IGenericUnitOfWork>(u => u.CommitAsync(), Times.Once);
    }
}
