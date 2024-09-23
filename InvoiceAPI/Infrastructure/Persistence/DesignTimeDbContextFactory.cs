using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvoiceAPI.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InvoiceDbContext>
{
    public InvoiceDbContext CreateDbContext(string[] args)
    {
        return new InvoiceDbContext(BaseDbContext<InvoiceDbContext>.DefaultOptions, new MockHttpContextAccessor());
    }

    public class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}