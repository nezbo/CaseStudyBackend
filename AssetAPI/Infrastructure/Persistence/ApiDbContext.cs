using AssetAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Infrastructure.Persistence;

public class ApiDbContext(DbContextOptions<ApiDbContext> options, IHttpContextAccessor http) 
    : BaseDbContext<ApiDbContext>(options, http)
{
    public DbSet<Asset> Assets { get; set; }
}
