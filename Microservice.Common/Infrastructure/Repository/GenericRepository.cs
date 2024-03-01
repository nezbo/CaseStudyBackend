using Microservice.Common.Application.Repository;
using Microservice.Common.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IIdentity, new()
{
    protected IBaseDbContext Context { get; }
    protected DbSet<T> DbSet => Context.GetSet<T>();

    public GenericRepository(IBaseDbContext dbContext)
    {
        Context = dbContext;
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        T instance = (Activator.CreateInstance(typeof(T)) as T)!;
        instance.Id = id;

        DbSet.Remove(instance);

        return Task.CompletedTask;
    }

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

    public Task UpdateAsync(T entity)
    {
        if (entity != null)
            DbSet.Update(entity);

        return Task.CompletedTask;
    }
}
