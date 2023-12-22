using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microservice.Common.CQRS;
using Microservice.Common.Features;
using Microservice.Common.Models;
using Microservice.Common.Repository;
using Microservice.Common.Test.Core;
using NSubstitute;

namespace Microservice.Common.Test;

public abstract class BasicCRUDCommandsHandlerTests<THandler, TApi, TDatabase> : BaseTestFixture<THandler>
    where THandler : BasicCRUDCommandsHandler<TApi, TDatabase>
    where TApi : class, IIdentity
    where TDatabase : class, IIdentity
{
    public BasicCRUDCommandsHandlerTests()
    {
        // IGenericUnitOfWork
        Container.Resolve<IGenericUnitOfWork>()
            .GetRepository<TDatabase>()
            .Returns(x => Container.Resolve<IGenericRepository<TDatabase>>());

        // IGenericRepository
        Container.Resolve<IGenericRepository<TDatabase>>()
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns(x => Task.FromResult<TDatabase?>(InstantiateDbEntity(x.Arg<Guid>())));

        // IMapper
        Container.Resolve<IMapper>()
            .Map<TDatabase>(Arg.Any<TApi>())
            .Returns(x => InstantiateDbEntity(x.Arg<TApi>().Id));
        Container.Resolve<IMapper>()
            .Map<TApi>(Arg.Any<TDatabase>())
            .Returns(x => InstantiateApiEntity(x.Arg<TDatabase>().Id));
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

        await Container.Resolve<IGenericRepository<TDatabase>>().Received(1).AddAsync(Arg.Is<TDatabase>(d => d.Id == id));
        await Container.Resolve<IGenericUnitOfWork>().Received(1).CommitAsync();
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

        await Container.Resolve<IGenericRepository<TDatabase>>().Received(1).GetByIdAsync(id);
        await Container.Resolve<IGenericRepository<TDatabase>>().Received(0).GetByIdAsync(Arg.Is<Guid>(i => i != id));
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
        Container.Resolve<IGenericRepository<TDatabase>>()
            .GetAllAsync()
            .Returns(entities);

        // Act
        IEnumerable<TApi> result = await Sut.Handle(new ListAllEntitiesQuery<TApi>(), CancellationToken.None);

        // Assert
        entities.Should().NotBeEmpty();
        entities.Should().HaveCount(2);

        await Container.Resolve<IGenericRepository<TDatabase>>().Received(1).GetAllAsync();
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
        Container.Resolve<IGenericRepository<TDatabase>>()
            .GetByIdsAsync(Arg.Any<Guid[]>())
            .Returns(x => entities.Where(e => x.Arg<Guid[]>().Contains(e.Id)));

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
        await Container.Resolve<IGenericRepository<TDatabase>>().Received(1).UpdateAsync(Arg.Is<TDatabase>(d => d.Id == id));
        await Container.Resolve<IGenericUnitOfWork>().Received(1).CommitAsync();
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
        await Container.Resolve<IGenericRepository<TDatabase>>().Received(1).DeleteAsync(id);
        await Container.Resolve<IGenericUnitOfWork>().Received(1).CommitAsync();
    }
}
