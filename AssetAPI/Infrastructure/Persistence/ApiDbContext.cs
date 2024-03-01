using AssetAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Infrastructure.Persistence;

public class ApiDbContext : BaseDbContext<ApiDbContext>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {

    }

    public DbSet<Asset> Assets { get; set; }
}
