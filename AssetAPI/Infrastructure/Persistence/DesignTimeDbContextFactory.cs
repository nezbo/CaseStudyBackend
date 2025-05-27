using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetAPI.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AssetDbContext>
{
    public AssetDbContext CreateDbContext(string[] args)
    {
        return new AssetDbContext(BaseDbContext<AssetDbContext>.DefaultOptions, new MockHttpContextAccessor());
    }

    public class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}