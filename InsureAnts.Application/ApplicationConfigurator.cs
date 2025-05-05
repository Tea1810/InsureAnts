using Mediator;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using InsureAnts.Application.Features.Abstractions;
using FluentValidation;

namespace InsureAnts.Application;

public static class ApplicationConfigurator
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.ConfigureMediator();

        return services;
    }
    private static void ConfigureMediator(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        services.AddPipelineHandlers(assembly);

        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Singleton, includeInternalTypes: true);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorHandlingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(InitializationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMediator(o => o.ServiceLifetime = ServiceLifetime.Transient);
    }

    private static void AddPipelineHandlers(this IServiceCollection services, Assembly assembly)
    {
        var interfaceTypes = new HashSet<Type>
        {
            typeof(IRequestInitializer<>),
            typeof(IErrorHandler<,>)
        };

        foreach (var assemblyType in assembly.GetTypes())
        {
            var typeInfo = assemblyType.GetTypeInfo();
            if (typeInfo is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            {
                var genericInterfaceType = typeInfo.GetInterfaces().FirstOrDefault(i => i.IsGenericType && interfaceTypes.Contains(i.GetGenericTypeDefinition()));
                if (genericInterfaceType != null)
                {
                    services.AddTransient(genericInterfaceType, assemblyType);
                }
            }
        }
    }
}
