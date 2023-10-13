using InvoiceAPI.External;
using InvoiceAPI.Persistence;
using InvoiceAPI.Persistence.Repositories;
using Microservice.Common.Extensions;
using Microservice.Common.MediatR.Validation;
using Microservice.Common.Repository;
using System.Reflection;

namespace InvoiceAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Add services to the container.

        builder.Services.AddBaseDbContext<InvoiceDbContext>();
        builder.Services.AddHttpClient();

        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient<IGenericUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddTransient<IServiceRepository, ServiceRepository>();

        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDateOnlyTimeOnlyStringConverters();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatRWithValidation(Assembly.GetExecutingAssembly());

        builder.Services.AddTransient<IAssetService, AssetService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.ApplyDatabaseMigrations<InvoiceDbContext>();

        app.Run();
    }
}
