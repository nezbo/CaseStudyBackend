using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IIdentity
{
    protected IBaseDbContext Context { get; }
    protected DbSet<T> DbSet => Context.GetSet<T>();

    public GenericRepository(IBaseDbContext dbContext)
	{
        this.Context = dbContext;
	}

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        T instance = (T)Activator.CreateInstance(typeof(T));
        instance.Id = id;

        DbSet.Remove(instance);

        return Task.CompletedTask;
    }

    public IAsyncEnumerable<T> GetAll()
    {
        return DbSet.AsAsyncEnumerable();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);

        return Task.CompletedTask;
    }
}
