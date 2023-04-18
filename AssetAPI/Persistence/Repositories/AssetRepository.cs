using AssetAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence.Repositories;

public class AssetRepository : GenericRepository<Asset>, IAssetRepository
{
    public AssetRepository(IBaseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn)
    {
        return DbSet.Where(a =>
            (a.ValidFrom == null || a.ValidFrom <= validOn)
            && (a.ValidTo == null || a.ValidTo >= validOn));
    }
}
