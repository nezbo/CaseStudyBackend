using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Infrastructure.Persistence;

public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options, IHttpContextAccessor httpContextAccessor)
        : BaseDbContext<InvoiceDbContext>(options, httpContextAccessor)
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>()
            .HasMany<Service>("_services")
            .WithOne(s => s.Invoice);
    }
}
