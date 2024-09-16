using ErrorOr;
using MediatR;
using Microservice.Common.Application.Features.Errors;
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
        await _repository.AddAsync(request.Entity);
        var changes = await _repository.SaveChangesAsync();

        return changes > 0
            ? request.Entity
            : CommonErrors.CreationFailed;
            
    }

    public virtual async Task<ErrorOr<TEntity>> Handle(GetEntityQuery<TEntity> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        return result == null
            ? Error.NotFound()
            : result;
    }

    public virtual async Task<ErrorOr<IEnumerable<TEntity>>> Handle(ListAllEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();
        return result == null
            ? Error.Unexpected()
            : ErrorOrFactory.From(result);
    }

    public virtual async Task<ErrorOr<IEnumerable<TEntity>>> Handle(ListEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdsAsync(request.Ids.ToArray());
        return result == null
            ? Error.Unexpected()
            : ErrorOrFactory.From(result);
    }

    public virtual async Task<ErrorOr<Updated>> Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(request.Entity);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0
            ? Result.Updated
            : CommonErrors.UpdateFailed;
    }

    public virtual async Task<ErrorOr<Deleted>> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return await _repository.SaveChangesAsync() > 0
            ? Result.Deleted
            : Error.NotFound();
    }
}
