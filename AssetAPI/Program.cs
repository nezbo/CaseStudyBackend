using AssetAPI.Application.Repository;
using AssetAPI.Infrastructure.Persistence;
using AssetAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Application.Extensions;
using Microservice.Common.Application.OpenTelemetry.Extensions;
using Microservice.Common.Application.Repository;
using Microservice.Common.Infrastructure.MediatR.Validation;
using Microservice.Common.Infrastructure.Repository;
using System.Reflection;

namespace AssetAPI
{
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

            builder.Services.AddBaseDbContext<ApiDbContext>();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IGenericUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IAssetRepository, AssetRepository>();

            builder.Services.AddProblemDetails();
            builder.Services.AddControllers()
                .AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDateOnlyTimeOnlyStringConverters();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddMediatRWithValidation(Assembly.GetExecutingAssembly());

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

            app.ApplyDatabaseMigrations<ApiDbContext>();

            app.Run();
        }
    }
}