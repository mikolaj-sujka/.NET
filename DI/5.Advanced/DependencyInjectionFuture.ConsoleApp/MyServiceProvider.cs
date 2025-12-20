using Jab;

namespace DependencyInjectionFuture.ConsoleApp
{
    [ServiceProvider]
    [Transient(typeof(IConsoleWriter), typeof(ConsoleWriter))]
    public partial class MyServiceProvider
    {
    }
}
