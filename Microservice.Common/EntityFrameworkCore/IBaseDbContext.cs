using Microservice.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.EntityFrameworkCore;

public interface IBaseDbContext
{
    Task<int> SaveChangesAsync(CancellationToken token = default);
    DbSet<T> GetSet<T>() where T : class, IIdentity;
}
