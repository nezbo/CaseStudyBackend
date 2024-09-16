using InvoiceAPI.Application.External;
using InvoiceAPI.Application.Repository;
using InvoiceAPI.Infrastructure.External;
using InvoiceAPI.Infrastructure.Persistence;
using InvoiceAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.Application.Repository;
using Microservice.Common.Infrastructure.MediatR.Validation;
using Microservice.Common.Infrastructure.Repository;
using System.Reflection;

namespace InvoiceAPI;

public class Program
{
    public static void Main(string[] args)
    {
        string serviceName = typeof(Program).Namespace!;
        var builder = WebApplication.CreateBuilder(args);
        builder.AddOpenTelemetry(serviceName, builder.Configuration.GetValue<string>("OTLP_Endpoint")!);

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Add services to the container.

        builder.Services.AddBaseDbContext<InvoiceDbContext>();
        builder.Services.AddHttpClient();

        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();

        builder.Services.AddProblemDetails();
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
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", serviceName);
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.ApplyDatabaseMigrations<InvoiceDbContext>();

        app.Run();
    }
}
