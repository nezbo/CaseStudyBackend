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

        modelBuilder.Entity<Invoice>().OwnsMany<Service>("_services", s =>
        {
            s.WithOwner().HasForeignKey(s => s.InvoiceId);
            s.Property<Guid>(nameof(Service.Id));
            s.HasKey(nameof(Service.Id));
        });
    }
}
