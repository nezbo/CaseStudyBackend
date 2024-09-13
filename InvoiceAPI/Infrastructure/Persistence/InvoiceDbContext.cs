using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Infrastructure.Persistence;

public class InvoiceDbContext : BaseDbContext<InvoiceDbContext>
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>().OwnsMany(i => i.Services, s =>
        {
            s.WithOwner().HasForeignKey(s => s.InvoiceId);
            s.Property<Guid>(nameof(Service.Id));
            s.HasKey(nameof(Service.Id));
        });
    }
}
