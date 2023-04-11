using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IIdentity
{
    private DbSet<T> _dbSet;

    public GenericRepository(IBaseDbContext dbContext)
	{
		this._dbSet = dbContext.GetSet<T>();
	}

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        T instance = (T)Activator.CreateInstance(typeof(T));
        instance.Id = id;

        _dbSet.Remove(instance);

        return Task.CompletedTask;
    }

    public IAsyncEnumerable<T> GetAll()
    {
        return _dbSet.AsAsyncEnumerable();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);

        return Task.CompletedTask;
    }
}
