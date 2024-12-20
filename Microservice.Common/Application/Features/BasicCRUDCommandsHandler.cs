﻿using ErrorOr;
using MediatR;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public abstract class BasicCRUDCommandsHandler<TEntity>
    (IMediator mediator, IGenericRepository<TEntity> repository)
    : IRequestHandler<CreateEntityCommand<TEntity>, ErrorOr<TEntity>>,
      IRequestHandler<GetEntityQuery<TEntity>, ErrorOr<TEntity>>,
      IRequestHandler<ListAllEntitiesQuery<TEntity>, ErrorOr<IEnumerable<TEntity>>>,
      IRequestHandler<ListEntitiesQuery<TEntity>, ErrorOr<IEnumerable<TEntity>>>,
      IRequestHandler<UpdateEntityCommand<TEntity>, ErrorOr<Updated>>,
      IRequestHandler<DeleteEntityCommand<TEntity>, ErrorOr<Deleted>>
    
    where TEntity : AggregateRoot
{
    private readonly IMediator _mediator = mediator;
    private readonly IGenericRepository<TEntity> _repository = repository;

    public virtual async Task<ErrorOr<TEntity>> Handle(CreateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var result = await _repository.AddAsync(request.Entity);

        if (!result.IsError)
            await SendIntegrationEvent("Created", request.Entity, cancellationToken);

        return request.Entity;
    }

    public virtual async Task<ErrorOr<TEntity>> Handle(GetEntityQuery<TEntity> request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }

    public virtual async Task<ErrorOr<IEnumerable<TEntity>>> Handle(ListAllEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<ErrorOr<IEnumerable<TEntity>>> Handle(ListEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdsAsync(request.Ids.ToArray());
    }

    public virtual async Task<ErrorOr<Updated>> Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var result = await _repository.UpdateAsync(request.Entity);

        if (!result.IsError)
            await SendIntegrationEvent("Updated", request.Entity, cancellationToken);

        return result;
    }

    public virtual async Task<ErrorOr<Deleted>> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var match = await _mediator.Send(new GetEntityQuery<TEntity>(request.Id), cancellationToken);

        if (match.IsError)
            return match.Errors;

        var result = await _repository.DeleteAsync(request.Id);

        if (!result.IsError)
            await SendIntegrationEvent("Deleted", match.Value, cancellationToken);

        return Result.Deleted;
    }

    private async Task SendIntegrationEvent(string eventName, TEntity entity, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new PublishEntityIntegrationEventCommand(eventName, "v1", entity), cancellationToken);
    }
}
