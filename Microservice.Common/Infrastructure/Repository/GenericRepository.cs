using Microservice.Common.Application.Repository;
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

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetByIdsAsync(params Guid[] ids)
    {
        return await DbSet
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(DbSet.AsEnumerable());
    }

    #endregion

    #region Write

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity != null)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }

    #endregion
}
