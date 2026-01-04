using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AdvancedTechniques.Tests.Unit;

public class DefaultBehaviorTests : IClassFixture<MyClassFixture>
{
    //private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;


    public DefaultBehaviorTests(ITestOutputHelper testOutputHelper, MyClassFixture myClassFixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = myClassFixture;
    }

    // Default behavior: Tests are running one after another in the same instance
    // But each test method gets its own instance of the test class
    // But in two different classes tests are running in parallel by default

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }
}
