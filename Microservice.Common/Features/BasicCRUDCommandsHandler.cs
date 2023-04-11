using AutoMapper;
using MediatR;
using Microservice.Common.CQRS;
using Microservice.Common.Models;
using Microservice.Common.Repository;
using Microservice.Common.Extensions;

namespace Microservice.Common.Features;

public abstract class BasicCRUDCommandsHandler<TApi,TDatabase> 
    : IRequestHandler<CreateEntityCommand<TApi>, Guid>,
      IRequestHandler<GetEntityQuery<TApi>, TApi>,
      IRequestHandler<ListEntitiesQuery<TApi>, IEnumerable<TApi>>,
      IRequestHandler<UpdateEntityCommand<TApi>>,
      IRequestHandler<DeleteEntityCommand<TApi>, bool>
    where TApi : IIdentity
    where TDatabase : IIdentity
{
    private readonly IGenericUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private IGenericRepository<TDatabase> Repository => _unitOfWork.GetRepository<TDatabase>();

    public BasicCRUDCommandsHandler(IGenericUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public virtual async Task<Guid> Handle(CreateEntityCommand<TApi> request, CancellationToken cancellationToken)
    {
        request.Entity.Id = Guid.NewGuid();

        TDatabase entity = _mapper.Map<TDatabase>(request.Entity);
        await Repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        return entity.Id;
    }

    public virtual async Task<TApi> Handle(GetEntityQuery<TApi> request, CancellationToken cancellationToken)
    {
        TDatabase entity = await Repository.GetByIdAsync(request.Id);
        return _mapper.Map<TApi>(entity);
    }

    public virtual async Task<IEnumerable<TApi>> Handle(ListEntitiesQuery<TApi> request, CancellationToken cancellationToken)
    {
        return await Repository.GetAll().SelectAsync(o => _mapper.Map<TApi>(o));
    }

    public virtual async Task Handle(UpdateEntityCommand<TApi> request, CancellationToken cancellationToken)
    {
        TDatabase entity = _mapper.Map<TDatabase>(request.Entity);
        await Repository.UpdateAsync(entity); 
        await _unitOfWork.CommitAsync();
    }

    public virtual async Task<bool> Handle(DeleteEntityCommand<TApi> request, CancellationToken cancellationToken)
    {
        await Repository.DeleteAsync(request.Id);
        return await _unitOfWork.CommitAsync() > 0;
    }
}
