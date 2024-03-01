using Microservice.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore;

public interface IBaseDbContext
{
    Task<int> SaveChangesAsync(CancellationToken token = default);
    DbSet<T> GetSet<T>() where T : class, IIdentity;
}
