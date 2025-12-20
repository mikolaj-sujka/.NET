namespace Vax;

public class ServiceDescriptor
{
    public Type ServiceType { get; init; } = default!;
    public Type? ImplementationType { get; set; } 
}