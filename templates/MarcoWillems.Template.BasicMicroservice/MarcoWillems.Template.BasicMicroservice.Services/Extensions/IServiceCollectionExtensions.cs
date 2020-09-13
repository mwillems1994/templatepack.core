using System.Linq;
using System.Reflection;
using MarcoWillems.Template.BasicMicroservice.Services.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MarcoWillems.Template.BasicMicroservice.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(
            this IServiceCollection services)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract
                    && t.GetCustomAttribute<DiClassAttribute>() != null)
                .ToArray();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<DiClassAttribute>()!;
                var lifetime = attribute.Lifetime;

                var interfaces = type.GetInterfaces();

                foreach (var i in interfaces)
                {
                    services.TryAdd(new ServiceDescriptor(i, type, lifetime));
                }

                if (!interfaces.Any())
                {
                    services.TryAdd(new ServiceDescriptor(type, type, lifetime));
                }
            }

            return services;
        }

        public static IServiceCollection AddCustomSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract
                    && t.GetCustomAttribute<DiSettingsAttribute>() != null)
                .ToArray();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<DiSettingsAttribute>()!;
                var name = attribute.Name ?? type.Name;

                var method = typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods()
                    .First(m => m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure)
                        && m.GetParameters().Length == 2);
                var generic = method.MakeGenericMethod(type);
                generic.Invoke(null, new object[] { services, configuration.GetSection(name) });
            }

            return services;
        }
    }
}
