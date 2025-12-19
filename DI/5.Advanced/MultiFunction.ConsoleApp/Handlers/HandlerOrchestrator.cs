using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MultiFunction.ConsoleApp.Handlers
{
    public class HandlerOrchestrator
    {
        private readonly Dictionary<string, Type> _handlerTypes = new();
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HandlerOrchestrator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            RegisterCommandHandler();
        }

        public IHandler? GetHandlerForCommandName(string command)
        {
            var handlerType = _handlerTypes.GetValueOrDefault(command);

            if (handlerType is null)
            {
                return null;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            return handler as IHandler;
        }

        private void RegisterCommandHandler()
        {
            var handlerTypes = HandlerExtensions.GetHandlerTypesFromAssembly(typeof(IHandler).Assembly);

            foreach (var handlerType in handlerTypes)
            {
                var commandNameAttribute = handlerType.GetCustomAttribute<CommandNameAttribute>();

                if (commandNameAttribute is null)
                {
                    continue;
                }

                var commandName = commandNameAttribute.CommandName;
                _handlerTypes[commandName] = handlerType.AsType();
            }
        }
    }
}
