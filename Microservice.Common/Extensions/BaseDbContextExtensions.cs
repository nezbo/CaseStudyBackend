﻿using Microservice.Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Common.Extensions;

public static class BaseDbContextExtensions
{
    public static void AddBaseDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext, IBaseDbContext
    {
        services.AddDbContext<TContext>();
        services.AddTransient<IBaseDbContext>(x => x.GetRequiredService<TContext>());
    }
}
