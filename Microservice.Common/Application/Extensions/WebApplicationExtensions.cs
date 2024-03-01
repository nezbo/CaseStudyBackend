using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Common.Application.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyDatabaseMigrations<TDbContext>(this WebApplication app) where TDbContext : DbContext
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<TDbContext>();
            context.Database.Migrate();
        }
    }
}
