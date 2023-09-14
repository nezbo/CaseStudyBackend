using AssetAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssetAPI.Persistence.Repositories;

public class AssetRepository : GenericRepository<Asset>, IAssetRepository
{
    public AssetRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Asset>> GetByIdsAsync(params Guid[] ids)
    {
        return await DbSet
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn)
    {
        return await DbSet.Where(a =>
            (a.ValidFrom == null || a.ValidFrom <= validOn)
            && (a.ValidTo == null || a.ValidTo >= validOn))
            .ToListAsync();
    }
}
