using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetAPI.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        return new ApiDbContext(BaseDbContext<ApiDbContext>.DefaultOptions, new MockHttpContextAccessor());
    }

    public class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}