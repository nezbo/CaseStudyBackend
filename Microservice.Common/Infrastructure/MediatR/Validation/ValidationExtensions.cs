using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microservice.Common.Infrastructure.MediatR.Validation;
public static class ValidationExtensions
{
    public static void AddMediatRWithValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
    }
}
