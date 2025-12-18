namespace Limitations.Api.Services
{
    public class SingletonService 
    {
        // Injecting a TransientService into a SingletonService
        // Singleton lifetime overshadows Transient lifetime
        // private readonly TransientService _transientService;


        // Injecting a ScopedService into a SingletonService
        // Using a Scoped service inside a Singleton can lead to Captive Dependency issues
        // And it is not allowed by the DI container by default, throwing an exception at runtime
        // private readonly ScopedService _scopedService;

        public Guid Id { get; } = Guid.NewGuid();
    }
}
