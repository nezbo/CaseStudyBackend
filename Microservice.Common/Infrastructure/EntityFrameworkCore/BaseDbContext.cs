using Microservice.Common.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore;

public abstract class BaseDbContext<TContext> : DbContext, IBaseDbContext
    where TContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseDbContext(IHttpContextAccessor httpContextAccessor) : base()
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public BaseDbContext(DbContextOptions<TContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<T> GetSet<T>() where T : class, IGetIdentity
    {
        return Set<T>();
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(e => e.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext!.Items
            .TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value)
                && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;

        return result;
    }
}
