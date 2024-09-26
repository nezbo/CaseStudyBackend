using MediatR;
using Microservice.Common.Application.Features.Events;
using Microservice.Common.Domain.Events.Producer;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
using Microservice.Common.Infrastructure.Events;
using Microservice.Common.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreLinq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microservice.Common;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure<TContext>(this IServiceCollection services, IConfiguration configuration, Assembly applicationAssembly)
        where TContext : DbContext, IBaseDbContext
    {
        services.AddServices(configuration, applicationAssembly)
            .AddPersistence<TContext>(applicationAssembly)
            .AddBackgroundServices(applicationAssembly);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, Assembly applicationAssembly)
    {
        services.AddHttpContextAccessor();
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(
            applicationAssembly, 
            typeof(DependencyInjection).Assembly));

        // Events
        var hostName = configuration["Messaging:HostName"];
        services.AddSingleton(new RabbitMQConnection(hostName!));
        services.Configure<RabbitMQSettings>(configuration.GetSection("Messaging"));
        services.AddSingleton<IIntegrationEventPublisher, RabbitMQEventPublisher>();
        services.AddSingleton<RabbitMQEventSubscriber>();

        return services;
    }

    public static IServiceCollection AddPersistence<TContext>(this IServiceCollection services, Assembly applicationAssembly)
        where TContext : DbContext, IBaseDbContext
    {
        // DbContext
        services.AddDbContext<TContext>(BaseDbContext<TContext>.ApplyDefaultOptions);
        services.AddScoped<DbContext>(x => x.GetRequiredService<TContext>());
        services.AddScoped<IBaseDbContext>(x => x.GetRequiredService<TContext>());

        // Repositories
        applicationAssembly.DefinedTypes
            .Where(t => t.BaseType != null && t.BaseType!.IsGenericType)
            .Where(t => t.BaseType!.GetGenericTypeDefinition() == typeof(GenericRepository<>))
            .ForEach(t => t.GetInterfaces().ForEach(i => services.AddScoped(i, t)));

        return services;
    }

    public static IServiceCollection AddBackgroundServices(this IServiceCollection services, Assembly applicationAssembly)
    {
        // Events
        services.AddHostedService<PublishIntegrationEventsWorker>();
        IEnumerable<(string,Type)> subscribedIntegrationEvents = ScanForIntegrationEventHandlers(applicationAssembly);
        subscribedIntegrationEvents.ForEach(t => AddIntegrationEventWorker(services, t.Item1, t.Item2));

        return services;
    }

    private static void AddIntegrationEventWorker(IServiceCollection collection, string eventKey, Type bodyType)
    {
        var methods = typeof(ServiceCollectionHostedServiceExtensions)
            .GetMethods();
        var addHostedServiceMethod = methods
            .Where(m => m.Name == "AddHostedService")
            .First(m => m.GetParameters().Length == 2); // with implementationFactory
        var addHostedServiceGenericMethod = addHostedServiceMethod
            .MakeGenericMethod(typeof(ReceiveIntegrationEventWorker<>)
                .MakeGenericType(bodyType));

        var factoryReturnType = typeof(ReceiveIntegrationEventWorker<>).MakeGenericType(bodyType);
        Expression<Func<IServiceProvider,object?>> factory = (IServiceProvider c) => typeof(DependencyInjection)
            .GetMethod(nameof(GetWorker))!
            .MakeGenericMethod(bodyType)
            .Invoke(null, new object?[] { c, eventKey });
        var typedFactory = Expression.Lambda(Expression.Convert(factory.Body, factoryReturnType), factory.Parameters).Compile();

        addHostedServiceGenericMethod.Invoke(null, [collection, typedFactory]);
    }

    public static ReceiveIntegrationEventWorker<TBody> GetWorker<TBody>(IServiceProvider services, string eventKey)
    {
        return new ReceiveIntegrationEventWorker<TBody>(
            eventKey,
            services.GetRequiredService<IMediator>(),
            services.GetRequiredService<RabbitMQEventSubscriber>(),
            services.GetRequiredService<IOptions<RabbitMQSettings>>(),
            services.GetRequiredService<ILogger<ReceiveIntegrationEventWorker<TBody>>>());
    }

    private static IEnumerable<(string,Type)> ScanForIntegrationEventHandlers(Assembly applicationAssembly)
    {
        return applicationAssembly.DefinedTypes
            .Where(t => t.GetCustomAttribute<IntegrationEventHandlerAttribute>() != null)
            .Select(t => (GetEventKeyFromAttribute(t), GetEventBodyTypeFromHandler(t)));
    }

    private static Type GetEventBodyTypeFromHandler(TypeInfo type)
    {

        var interfaces = type.GetInterfaces()
            .Union(type.BaseType?.GetInterfaces() ?? []);

        var match = interfaces
            .First(i => i.Name.Contains("INotificationHandler"));

        return match
            .GenericTypeArguments[0] // ReceivedIntegrationEvent<TBody>
            .GenericTypeArguments[0]; // TBody
    }

    private static string GetEventKeyFromAttribute(TypeInfo type)
    {
        var attr = type.GetCustomAttribute<IntegrationEventHandlerAttribute>()!;
        return $"{attr.EventName}.{attr.EventVersion}";
    }

    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}
