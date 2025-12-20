namespace Vax;

public class ServiceProvider : IServiceProvider
{
    internal ServiceProvider(ServiceCollection serviceCollection)
    {

    }
    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    object? IServiceProvider.GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }
}