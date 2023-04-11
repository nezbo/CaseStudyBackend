using AssetAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Persistence;

public class ApiDbContext : DbContext, IBaseDbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {

    }

    public DbSet<Asset> Assets { get; set; }

    public DbSet<T> GetSet<T>() where T : class, IIdentity
    {
        return this.Set<T>();
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
