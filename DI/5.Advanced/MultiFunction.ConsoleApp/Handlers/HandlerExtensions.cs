using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MultiFunction.ConsoleApp.Handlers
{
    public static class HandlerExtensions
    {
        public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
        {
            services.AddSingleton<HandlerOrchestrator>();

            var handlerTypes = GetHandlerTypesFromAssembly(assembly);

            foreach (var handlerType in handlerTypes)
            {
                services.TryAddTransient(handlerType.AsType());
            }
        }

        public static IEnumerable<TypeInfo> GetHandlerTypesFromAssembly(Assembly assembly)
        {
            var handlerTypes = typeof(IHandler).Assembly.DefinedTypes
                .Where(x => x is { IsInterface: false, IsAbstract: false } && typeof(IHandler).IsAssignableFrom(x));
            return handlerTypes;
        }
    }
}
