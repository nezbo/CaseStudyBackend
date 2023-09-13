using AssetAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Persistence;

public class ApiDbContext : BaseDbContext<ApiDbContext>
{
    public DbSet<Asset> Assets { get; set; }
}
