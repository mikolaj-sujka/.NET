using System;

namespace AdvancedTechniques.Tests.Unit;

// Fixture class to demonstrate shared context across tests
public class MyClassFixture : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();

    public MyClassFixture()
    {

    }

    public void Dispose()
    {

    }
}
