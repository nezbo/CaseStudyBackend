using ErrorOr;
using MediatR;
using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Events;
using Microservice.Common.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Infrastructure.Repository;

public class GenericRepository<T>(IBaseDbContext dbContext)
    : IGenericRepository<T>
    where T : Entity
{
    protected IBaseDbContext Context { get; } = dbContext;

    protected DbSet<T> DbSet => Context.GetSet<T>();

    #region Read

    public async Task<ErrorOr<T>> GetByIdAsync(Guid id)
    {
        var match = await DbSet.FindAsync(id);
        if (match == null)
            return NotFoundError(id);
        return match;
    }

    public async Task<ErrorOr<IEnumerable<T>>> GetByIdsAsync(params Guid[] ids)
    {
        var matches = await DbSet
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();

        if (matches.Count < ids.Length)
        {
            return ids
                .Except(matches.Select(v => v.Id))
                .Select(NotFoundError)
                .ToList();
        }

        return matches;
    }

    public Task<ErrorOr<IEnumerable<T>>> GetAllAsync()
    {
        return Task.FromResult(DbSet.AsEnumerable().ToErrorOr());
    }

    #endregion

    private static Error NotFoundError(Guid id)
    {
        return Error.NotFound(
                            description: $"No {typeof(T).Name} found for id = {id}",
                            metadata: new Dictionary<string, object>() { { "id", id } });
    }

    #region Write

    public async Task<ErrorOr<Created>> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return (await Context.SaveChangesAsync()) > 0
            ? Result.Created
            : Error.Failure(
                description: $"Failed to create {typeof(T).Name} with id = {entity?.Id}",
                metadata: new Dictionary<string, object>() { { "id", entity?.Id ?? default } });
    }

    public async Task<ErrorOr<Updated>> UpdateAsync(T entity)
    {
        if (entity != null)
        {
            DbSet.Update(entity);
            try
            {
                if (await Context.SaveChangesAsync() > 0)
                    return Result.Updated;
            }
            catch(Exception) { }
        }

        return Error.Failure(
            description: $"Failed to update {typeof(T).Name} with id = {entity?.Id}",
            metadata: new Dictionary<string, object>() { { "id", entity?.Id ?? default } });
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            if (await Context.SaveChangesAsync() > 0)
                return Result.Deleted;
        }

        return Error.Failure(
            description: $"Failed to delete {typeof(T).Name} with id = {id}",
            metadata: new Dictionary<string, object>() { { "id", entity?.Id ?? default } });
    }

    #endregion
}
