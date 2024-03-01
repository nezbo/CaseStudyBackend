using InvoiceAPI.Domain.Models;
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Infrastructure.Persistence;

public class InvoiceDbContext : BaseDbContext<InvoiceDbContext>
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }
}
