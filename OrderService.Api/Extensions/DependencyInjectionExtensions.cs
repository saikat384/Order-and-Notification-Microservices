using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OrderService.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddTransientFromAssembly(this IServiceCollection services, Type generalType, Assembly assembly)
    {
        assembly.GetTypes()
            .Where(item => item.GetInterfaces()
                               .Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == generalType) &&
                           item is { IsAbstract: false, IsInterface: false })
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == generalType);
                services.AddTransient(serviceType, assignedTypes);
            });
    }

    public static void AddDecorator(this IServiceCollection services, Type interfaceType, Type decoratorType)
    {
        foreach (var decoratedDescriptor in services.Where(descriptor => TypeEquals(descriptor.ServiceType, interfaceType)).ToArray())
        {
            var serviceType = decoratedDescriptor.ServiceType;

            var enhancedType = EnhanceType(decoratorType, serviceType);

            var objectFactory = ActivatorUtilities.CreateFactory(enhancedType, [serviceType]);

            services.Replace(ServiceDescriptor.Describe(
                serviceType,
                s => objectFactory(s, [s.CreateInstance(decoratedDescriptor)]),
                decoratedDescriptor.Lifetime)
            );
        }
    }

    private static bool TypeEquals(Type serviceType, Type interfaceType)
    {
        return serviceType == interfaceType ||
               (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == interfaceType);
    }

    private static Type EnhanceType(Type decoratorType, Type serviceType)
    {
        if (!serviceType.IsGenericType)
        {
            return decoratorType;
        }

        var genericParameters = serviceType.GenericTypeArguments;
        return decoratorType.MakeGenericType(genericParameters);
    }

    private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
    {
        var implementationInstance = descriptor.ImplementationInstance;
        if (implementationInstance != null)
        {
            return implementationInstance;
        }

        var implementationFactory = descriptor.ImplementationFactory;
        if (implementationFactory != null)
        {
            return implementationFactory(services);
        }

        var implementationType = descriptor.ImplementationType;
        return implementationType == null
            ? throw new NullReferenceException($"ImplementationType for {descriptor.ServiceType} not found!")
            : ActivatorUtilities.GetServiceOrCreateInstance(services, implementationType);
    }
}