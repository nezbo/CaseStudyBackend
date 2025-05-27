using AssetAPI.Infrastructure.Persistence;
using InvoiceAPI.Infrastructure.Persistence;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.DI;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace ModularMonolith;

public class Program
{
    public static void Main(string[] args)
    {
        string serviceName = typeof(Program).Namespace!;
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Add services to the container.
        List<Assembly> moduleAssemblies = [ typeof(AssetAPI.Program).Assembly, typeof(InvoiceAPI.Program).Assembly ];
        var partManager = builder.Services.AddControllers().PartManager;
        moduleAssemblies.ForEach(a => partManager.ApplicationParts.Add(new AssemblyPart(a)));

        builder.Services.AddHttpClient();
        builder.AddOpenTelemetry(serviceName, builder.Configuration.GetValue<string>("OTLP_Endpoint")!);
        builder.Services.AddInfrastructureServices(builder.Configuration, moduleAssemblies)
            .AddPersistence<AssetDbContext>()
            .AddPersistence<InvoiceDbContext>();

        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDateOnlyTimeOnlyStringConverters();

        builder.Services.AddServiceRegistrationsFromAssemblies(builder.Configuration, moduleAssemblies);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ModularMonolith API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.ApplyDatabaseMigrations<AssetDbContext>();
        app.ApplyDatabaseMigrations<InvoiceDbContext>();
        app.AddInfrastructureMiddleware();

        app.Run();
    }
}
