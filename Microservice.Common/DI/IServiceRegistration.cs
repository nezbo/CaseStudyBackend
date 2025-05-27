using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Common.DI;
public interface IServiceRegistration
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
}
