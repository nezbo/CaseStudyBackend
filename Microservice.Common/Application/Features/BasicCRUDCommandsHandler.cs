using ErrorOr;
using MediatR;
using Microservice.Common.Application.Features.Errors;
using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public abstract class BasicCRUDCommandsHandler<TDatabase>
    (IMediator mediator, IGenericRepository<TDatabase> repository)
    : IRequestHandler<CreateEntityCommand<TDatabase>, ErrorOr<TDatabase>>,
      IRequestHandler<GetEntityQuery<TDatabase>, ErrorOr<TDatabase>>,
      IRequestHandler<ListAllEntitiesQuery<TDatabase>, ErrorOr<IEnumerable<TDatabase>>>,
      IRequestHandler<ListEntitiesQuery<TDatabase>, ErrorOr<IEnumerable<TDatabase>>>,
      IRequestHandler<UpdateEntityCommand<TDatabase>, ErrorOr<Updated>>,
      IRequestHandler<DeleteEntityCommand<TDatabase>, ErrorOr<Deleted>>
    
    where TDatabase : Entity
{
    private readonly IMediator _mediator = mediator;
    private readonly IGenericRepository<TDatabase> _repository = repository;

    public virtual async Task<ErrorOr<TDatabase>> Handle(CreateEntityCommand<TDatabase> request, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(request.Entity);
        var changes = await _repository.SaveChangesAsync();

        return changes > 0
            ? request.Entity
            : CommonErrors.CreationFailed;
            
    }

    public virtual async Task<ErrorOr<TDatabase>> Handle(GetEntityQuery<TDatabase> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        return result == null
            ? Error.NotFound()
            : result;
    }

    public virtual async Task<ErrorOr<IEnumerable<TDatabase>>> Handle(ListAllEntitiesQuery<TDatabase> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();
        return result == null
            ? Error.Unexpected()
            : ErrorOrFactory.From(result);
    }

    public virtual async Task<ErrorOr<IEnumerable<TDatabase>>> Handle(ListEntitiesQuery<TDatabase> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdsAsync(request.Ids.ToArray());
        return result == null
            ? Error.Unexpected()
            : ErrorOrFactory.From(result);
    }

    public virtual async Task<ErrorOr<Updated>> Handle(UpdateEntityCommand<TDatabase> request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(request.Entity);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0
            ? Result.Updated
            : CommonErrors.UpdateFailed;
    }

    public virtual async Task<ErrorOr<Deleted>> Handle(DeleteEntityCommand<TDatabase> request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return await _repository.SaveChangesAsync() > 0
            ? Result.Deleted
            : Error.NotFound();
    }
}
