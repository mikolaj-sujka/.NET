namespace Vax;

public class ServiceCollection : List<ServiceDescriptor>
{
    public ServiceCollection AddSingleton<TService, TImplementation>()
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
        };

        Add(serviceDescriptor);

        return this;
    }

    public IServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(this);
    }
}