using AssetAPI.Application.Repository;
using AssetAPI.Infrastructure.Persistence;
using AssetAPI.Infrastructure.Persistence.Repository;
using Microservice.Common.Application.Extensions;
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
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddBaseDbContext<ApiDbContext>();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IGenericUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IAssetRepository, AssetRepository>();

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
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.ApplyDatabaseMigrations<ApiDbContext>();

            app.Run();
        }
    }
}