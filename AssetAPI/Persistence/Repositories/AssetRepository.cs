using AssetAPI.Models.Database;
using Microservice.Common.Repository;

namespace AssetAPI.Persistence.Repositories;

public class AssetRepository : GenericRepository<Asset>, IAssetRepository
{
    private readonly ApiDbContext _dbContext;

    public AssetRepository(ApiDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Asset>> GetValidOnAsync(DateOnly validOn)
    {
        return _dbContext.Assets.Where(a =>
            (a.ValidFrom == null || a.ValidFrom <= validOn)
            && (a.ValidTo == null || a.ValidTo >= validOn));
    }
}
