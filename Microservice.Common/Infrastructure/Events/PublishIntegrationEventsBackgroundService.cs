using Microservice.Common.Domain.Events;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Microservice.Common.Infrastructure.Events;
public class PublishIntegrationEventsBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    IIntegrationEventPublisher integrationEventPublisher,
    ILogger<PublishIntegrationEventsBackgroundService> logger) 
    : IHostedService
{
    private Task? _doWorkTask = null;
    private PeriodicTimer? _timer = null!;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IIntegrationEventPublisher _integrationEventPublisher = integrationEventPublisher;
    private readonly ILogger<PublishIntegrationEventsBackgroundService> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _doWorkTask = DoWorkAsync();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_doWorkTask is null)
            return;

        _cts.Cancel();
        await _doWorkTask;

        _timer?.Dispose();
        _cts.Dispose();
    }

    private async Task DoWorkAsync()
    {
        _logger.LogInformation("Starting integration event publisher background service.");

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while(await _timer.WaitForNextTickAsync(_cts.Token))
        {
            try
            {
                await PublishIntegrationEventsFromDbAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception occurred while publishing integration events.");
            }
        }
    }

    private async Task PublishIntegrationEventsFromDbAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IBaseDbContext>();
        var dbSet = dbContext.GetSet<IntegrationEvent>();
        var events = dbSet.ToList();

        if (events.Count != 0)
        {
            _logger.LogInformation("Read a total of {NumEvents} outbox integration events", events.Count);

            foreach (var intEvent in events)
            {
                await _integrationEventPublisher.PublishAsync(intEvent);
            }

            dbSet.RemoveRange(events);
            await dbContext.SaveChangesAsync();
        }
    }
}
