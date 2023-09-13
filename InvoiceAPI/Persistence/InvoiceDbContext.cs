using InvoiceAPI.Models.Database;
using Microservice.Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Persistence;

public class InvoiceDbContext : BaseDbContext<InvoiceDbContext>
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }
}
