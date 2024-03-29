﻿using Microservice.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microservice.Common.Infrastructure.EntityFrameworkCore;

public abstract class BaseDbContext<TContext> : DbContext, IBaseDbContext
    where TContext : DbContext
{
    public BaseDbContext() : base()
    {

    }

    public BaseDbContext(DbContextOptions<TContext> options) : base(options)
    {

    }

    public DbSet<T> GetSet<T>() where T : class, IIdentity
    {
        return Set<T>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("WebApiDatabase");
        optionsBuilder.UseSqlite(connectionString);
    }
}
