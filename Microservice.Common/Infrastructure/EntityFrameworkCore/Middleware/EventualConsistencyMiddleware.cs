using MediatR;
using Microservice.Common.Domain.Events;
using Microservice.Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, DbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value)
                    && value is Queue<IDomainEvent> domainEvents)
                {
                    while(domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }
                
                await transaction.CommitAsync();
            }
            catch(EventualConsistencyException)
            {
                // TODO Handle this gently
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
