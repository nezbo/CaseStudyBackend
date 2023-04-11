using System.Reflection;
using AssetAPI.Persistence;
using AssetAPI.Persistence.Repositories;
using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Extensions;
using Microservice.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");
            builder.Services.AddDbContext<ApiDbContext>(x => x.UseSqlite(connectionString));
            builder.Services.AddTransient<IBaseDbContext>(x => x.GetRequiredService<ApiDbContext>());

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IGenericUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IAssetRepository, AssetRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDateOnlyTimeOnlyStringConverters();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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

            app.ApplyDatabaseMigrations<ApiDbContext>();

            app.Run();
        }
    }
}