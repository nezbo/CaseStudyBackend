using AssetAPI.Infrastructure.Persistence;
using InvoiceAPI.Application.External;
using InvoiceAPI.Domain.Models;
using InvoiceAPI.Infrastructure.Persistence;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.DI;
using Microservice.Common.Domain.Events.Producer;
using Microservice.Common.Infrastructure.Events.Workers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Data.Sqlite;
using ModularMonolith.Infrastructure;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        var partManager = builder.Services.AddControllers()
            .AddControllersAsServices()
            .PartManager;
        moduleAssemblies.ForEach(a => partManager.ApplicationParts.Add(new AssemblyPart(a)));

        builder.AddOpenTelemetry(serviceName, builder.Configuration.GetValue<string>("OTLP_Endpoint")!, AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddHttpClient();

        builder.Services.AddInfrastructureServices(builder.Configuration, moduleAssemblies)
            .AddPersistence<AssetDbContext>(builder.Configuration, OpenSqliteConnection(builder.Configuration, nameof(AssetDbContext)))
            .AddPersistence<InvoiceDbContext>(builder.Configuration, OpenSqliteConnection(builder.Configuration, nameof(InvoiceDbContext)));
        builder.Services.AddHostedService<PublishIntegrationEventsWorker>();

        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDateOnlyTimeOnlyStringConverters();

        builder.Services.AddServiceRegistrationsFromAssemblies(builder.Configuration, moduleAssemblies);

        // Modular Monolith Overrides
        builder.Services.AddTransient<IAssetService, InProcessAssetService>();
        builder.Services.AddSingleton<IIntegrationEventPublisher, MonolithicIntegrationEventPublisher>();

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

        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.AddInfrastructureMiddleware();
        app.MapControllers();

        app.ApplyDatabaseMigrations<AssetDbContext>();
        app.ApplyDatabaseMigrations<InvoiceDbContext>();

        app.Run();
    }

    private static SqliteConnection OpenSqliteConnection(ConfigurationManager configuration, string name)
    {
        var connectionString = configuration.GetConnectionString("WebApiDatabase")
            !.Replace(".db", $"_{name}.db", StringComparison.OrdinalIgnoreCase);
        var sqliteConnection = new SqliteConnection(connectionString);
        sqliteConnection.Open(); // Keep the connection open for the app's lifetime

        return sqliteConnection;
    }
}
