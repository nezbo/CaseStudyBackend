using AssetAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Infrastructure.Persistence;

public class AssetDbContext(DbContextOptions<AssetDbContext> options, IHttpContextAccessor http) 
    : BaseDbContext<AssetDbContext>(options, http)
{
    public DbSet<Asset> Assets { get; set; }
}
