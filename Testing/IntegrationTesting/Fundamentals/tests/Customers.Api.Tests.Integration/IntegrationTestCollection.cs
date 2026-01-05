using Microsoft.AspNetCore.Mvc.Testing;

namespace Customers.Api.Tests.Integration;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<WebApplicationFactory<IApiMarker>>
{
}