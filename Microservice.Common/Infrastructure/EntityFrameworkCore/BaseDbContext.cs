using Microservice.Common.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore;

public abstract class BaseDbContext<TContext>(DbContextOptions<TContext> options, IHttpContextAccessor httpContextAccessor) 
    : DbContext(options), IBaseDbContext
    where TContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public static DbContextOptions<TContext> DefaultOptions 
    { 
        get 
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            ApplyDefaultOptions(builder);
            return builder.Options; 
        } 
    }
    
    public static void ApplyDefaultOptions(DbContextOptionsBuilder options)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("WebApiDatabase");
        options.UseSqlite(connectionString);
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
