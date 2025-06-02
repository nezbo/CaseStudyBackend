using MediatR;
using Microservice.Common.Domain.Events;
using Microservice.Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, IServiceProvider serviceProvider)
    {
        // Find all registered DbContexts in the current request scope
        var dbContexts = serviceProvider
            .GetServices<DbContext>()
            .Distinct()
            .ToList();

        // Begin a transaction for each DbContext
        var transactions = new List<IDbContextTransaction>();
        foreach (var dbContext in dbContexts)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            transactions.Add(transaction);
        }

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value)
                    && value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                // Save changes for all DbContexts
                foreach (var dbContext in dbContexts)
                {
                    if (dbContext.ChangeTracker.HasChanges())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                }

                // Commit all transactions
                foreach (var transaction in transactions)
                {
                    await transaction.CommitAsync();
                }
            }
            catch (EventualConsistencyException)
            {
                // TODO Handle this gently
            }
            finally
            {
                // Dispose all transactions
                foreach (var transaction in transactions)
                {
                    await transaction.DisposeAsync();
                }
            }
        });

        await _next(context);
    }
}
