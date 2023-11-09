using AssetAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence.Repositories;

public class AssetRepository : GenericRepository<Asset>, IAssetRepository
{
    public AssetRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<Asset>> GetByIdsAsync(params Guid[] ids)
    {
        var query = () => DbSet
            .Where(a => ids.Contains(a.Id))
            .ToList()
            .AsEnumerable();

        return Task.Run(query);
    }

    public Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn)
    {
        var query = () => DbSet.Where(a =>
            (a.ValidFrom == null || a.ValidFrom <= validOn)
            && (a.ValidTo == null || a.ValidTo >= validOn))
            .ToList()
            .AsEnumerable();

        return Task.Run(query);
    }
}
