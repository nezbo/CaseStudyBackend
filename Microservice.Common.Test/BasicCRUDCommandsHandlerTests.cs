using FluentAssertions;
using FluentValidation;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Models;
using Microservice.Common.Test.Core;
using NSubstitute;

namespace Microservice.Common.Test;

public abstract class BasicCRUDCommandsHandlerTests<THandler, TDomain> : BaseTestFixture<THandler>
    where THandler : BasicCRUDCommandsHandler<TDomain>
    where TDomain : AggregateRoot
{
    public BasicCRUDCommandsHandlerTests()
    {
        // IGenericRepository
        Container.Resolve<IGenericRepository<TDomain>>()
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns(x => Task.FromResult<TDomain?>(InstantiateEntity(x.Arg<Guid>())));

        Container.Resolve<IGenericRepository<TDomain>>()
            .SaveChangesAsync()
            .Returns(1);
    }

    protected abstract TDomain InstantiateEntity(Guid id);

    [Fact]
    public async Task Handle_CreateEntityCommand_ShouldCreateEntity()
    {
        // Arrange
        var entity = InstantiateEntity(Guid.NewGuid());
        var command = new CreateEntityCommand<TDomain>(entity);

        // Act
        var response = await Sut.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Value.Id.Should().Be(entity.Id);

        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).AddAsync(Arg.Is<TDomain>(d => d.Id == entity.Id));
        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_GetEntityQuery_ShouldReturnEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        var response = await Sut.Handle(new GetEntityQuery<TDomain>(id), CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Value.Id.Should().Be(id);

        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).GetByIdAsync(id);
        await Container.Resolve<IGenericRepository<TDomain>>().Received(0).GetByIdAsync(Arg.Is<Guid>(i => i != id));
    }

    [Fact]
    public async Task Handle_ListAllEntitiesQuery_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = new[]
        {
            InstantiateEntity(Guid.NewGuid()),
            InstantiateEntity(Guid.NewGuid()),
        };
        Container.Resolve<IGenericRepository<TDomain>>()
            .GetAllAsync()
            .Returns(entities);

        // Act
        var response = await Sut.Handle(new ListAllEntitiesQuery<TDomain>(), CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Value.Should().HaveCount(2);

        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).GetAllAsync();
    }

    [Fact]
    public async Task Handle_ListEntitiesQuery_ShouldReturnEntities()
    {
        // Arrange
        var entities = new[]
        {
            InstantiateEntity(Guid.NewGuid()),
            InstantiateEntity(Guid.NewGuid()),
        };
        Container.Resolve<IGenericRepository<TDomain>>()
            .GetByIdsAsync(Arg.Any<Guid[]>())
            .Returns(x => entities.Where(e => x.Arg<Guid[]>().Contains(e.Id)));

        // Act
        var query = entities.Select(e => e.Id).Append(Guid.NewGuid()).ToArray();
        var response = await Sut.Handle(new ListEntitiesQuery<TDomain>(query), CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ListEntitiesQuery_ShouldReturnEmpty_WhenNoInds()
    {
        var response = await Sut.Handle(new ListEntitiesQuery<TDomain>([]), CancellationToken.None);
        response.IsError.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_UpdateEntityCommand_ShouldUpdateEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var updateEntity = InstantiateEntity(id);
        var command = new UpdateEntityCommand<TDomain>(id, updateEntity);

        // Act
        await Sut.Handle(command, CancellationToken.None);

        // Assert
        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).UpdateAsync(Arg.Is<TDomain>(d => d.Id == id));
        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_DeleteEntityCommand_ShouldDeleteEntity()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var command = new DeleteEntityCommand<TDomain>(id);

        // Act
        await Sut.Handle(command, CancellationToken.None);

        // Assert
        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).DeleteAsync(id);
        await Container.Resolve<IGenericRepository<TDomain>>().Received(1).SaveChangesAsync();
    }
}
