using Microservice.Common.Application.Repository;
using Microservice.Common.Extensions;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.EntityFrameworkCore.Middleware;
using Microservice.Common.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MoreLinq;
using System.Reflection;

namespace Microservice.Common;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure<TContext>(this IServiceCollection services, Assembly applicationAssembly)
        where TContext : DbContext, IBaseDbContext
    {
        services.AddServices(applicationAssembly)
            .AddPersistence<TContext>(applicationAssembly);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, Assembly applicationAssembly)
    {
        services.AddHttpContextAccessor();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(applicationAssembly));

        return services;
    }

    public static IServiceCollection AddPersistence<TContext>(this IServiceCollection services, Assembly applicationAssembly)
        where TContext : DbContext, IBaseDbContext
    {
        // DbContext
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("WebApiDatabase");

        services.AddDbContext<TContext>(o => o.UseSqlite(connectionString));
        services.AddScoped<DbContext>(x => x.GetRequiredService<TContext>());
        services.AddScoped<IBaseDbContext>(x => x.GetRequiredService<TContext>());

        // Repositories
        applicationAssembly.DefinedTypes
            .Where(t => t.BaseType != null && t.BaseType!.IsGenericType)
            .Where(t => t.BaseType!.GetGenericTypeDefinition() == typeof(GenericRepository<>))
            .ForEach(t => t.GetInterfaces().ForEach(i => services.AddScoped(i, t)));

        return services;
    }

    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}
