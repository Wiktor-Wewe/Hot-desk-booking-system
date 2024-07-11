using MediatR;
using System.Reflection;

namespace Hot_desk_booking_system.Extensions
{
    public static class MediatRServiceExtensions
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))))
                    .ToList();

                foreach (var handlerType in handlerTypes)
                {
                    var interfaceTypes = handlerType.GetInterfaces()
                        .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
                        .ToList();

                    foreach (var interfaceType in interfaceTypes)
                    {
                        services.AddTransient(interfaceType, handlerType);
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                // Register repositories
                var repositoryTypes = types
                    .Where(t => t.Name.EndsWith("Repository") && t.IsClass && !t.IsAbstract)
                    .ToList();

                foreach (var repositoryType in repositoryTypes)
                {
                    var interfaceType = repositoryType.GetInterfaces().FirstOrDefault(i => i.Name == "I" + repositoryType.Name);
                    if (interfaceType != null)
                    {
                        services.AddScoped(interfaceType, repositoryType);
                    }
                }

                // Register services
                var serviceTypes = types
                    .Where(t => t.Name.EndsWith("Service") && t.IsClass && !t.IsAbstract)
                    .ToList();

                foreach (var serviceType in serviceTypes)
                {
                    var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name == "I" + serviceType.Name);
                    if (interfaceType != null)
                    {
                        services.AddScoped(interfaceType, serviceType);
                    }
                }
            }

            return services;
        }
    }
}
