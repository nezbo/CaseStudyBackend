using AssetAPI.Infrastructure.Persistence;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.DI;
using System.Reflection;

namespace AssetAPI;

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

        builder.AddOpenTelemetry(serviceName, builder.Configuration.GetValue<string>("OTLP_Endpoint")!);
        builder.Services.AddInfrastructure<AssetDbContext>(builder.Configuration, Assembly.GetExecutingAssembly());

        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDateOnlyTimeOnlyStringConverters();

        builder.Services.AddServiceRegistrationsFromAssemblies(builder.Configuration, Assembly.GetExecutingAssembly());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", serviceName);
                options.RoutePrefix = string.Empty;
            });
        }

        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.ApplyDatabaseMigrations<AssetDbContext>();
        app.AddInfrastructureMiddleware();

        app.Run();
    }
}