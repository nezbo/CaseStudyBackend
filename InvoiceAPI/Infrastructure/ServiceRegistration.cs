using InvoiceAPI.Application.External;
using InvoiceAPI.Infrastructure.External;
using Microservice.Common.DI;

namespace InvoiceAPI.Infrastructure;

public class ServiceRegistration : IServiceRegistration
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAssetService, AssetService>();
    }
}
