using MediatR;
using Microservice.Common.Domain.Events;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;

namespace Microservice.Common.Application.Features.Events;
internal class IntegrationEventHandler<TIntegrationEvent>(IBaseDbContext dbContext, IHttpContextAccessor httpContextAccessor) 
    : INotificationHandler<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    private readonly IBaseDbContext _dbContext = dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task Handle(TIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        integrationEvent.TraceId = Activity.Current?.Id;
        if (_httpContextAccessor?.HttpContext != null)
            integrationEvent.Source = new Uri(_httpContextAccessor.HttpContext.Request.GetDisplayUrl());

        await _dbContext.GetSet<IntegrationEvent>().AddAsync(integrationEvent);
        await _dbContext.SaveChangesAsync();
    }
}
