using AssetAPI.Application.Repository;
using AssetAPI.Domain.Models;
using ErrorOr;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace AssetAPI.Infrastructure.Persistence.Repository;

public class AssetRepository(IBaseDbContext dbContext) 
    : GenericRepository<Asset>(dbContext), IAssetRepository
{
    public new Task<ErrorOr<IEnumerable<Asset>>> GetByIdsAsync(params Guid[] ids)
    {
        var query = () => DbSet
            .Where(a => ids.Contains(a.Id))
            .ToList()
            .AsEnumerable()
            .ToErrorOr();

        return Task.Run(query);
    }

    public Task<ErrorOr<IEnumerable<Asset>>> GetValidOnAsync(DateOnly validOn)
    {
        var query = () => DbSet.Where(a =>
            (a.ValidFrom == null || a.ValidFrom <= validOn)
            && (a.ValidTo == null || a.ValidTo >= validOn))
            .ToList()
            .AsEnumerable()
            .ToErrorOr();

        return Task.Run(query);
    }
}
