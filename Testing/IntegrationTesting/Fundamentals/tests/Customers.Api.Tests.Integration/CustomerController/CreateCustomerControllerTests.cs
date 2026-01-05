using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Customers.Api.Tests.Integration.CustomerController
{
    // WebApplicationFactory is used to create a test server for integration testing
    // IApiMarker is a marker interface in the Customers.Api project
    // that helps to identify the assembly for the WebApplicationFactory

    // IClassFixture is used to share the WebApplicationFactory instance
    // across multiple test methods in the test class

    // IAsyncLifetime is used to perform async setup and teardown for the test class

    [Collection("IntegrationTestCollection")]
    public class CreateCustomerControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        private readonly HttpClient _client = appFactory.CreateClient();

        private readonly Faker<CustomerRequest> _costumerGenerator =
            new Faker<CustomerRequest>()
                .RuleFor(x => x.FullName, faker => faker.Person.FullName)
                .RuleFor(x => x.Email, faker => faker.Person.Email)
                .RuleFor(x => x.GitHubUsername, "nickchapsas")
                .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

        private readonly List<Guid> _createdIds = new List<Guid>();

        [Fact]
        public async Task Create_ReturnsCreatedCustomer_WhenRequestIsValid()
        {
            // Arrange
            var customerRequest = _costumerGenerator.Generate();
            
            // Act
            var response = await _client.PostAsJsonAsync("/customers", customerRequest);
            
            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            var createdCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
            createdCustomer.ShouldNotBeNull();
            createdCustomer!.Id.ShouldNotBe(Guid.Empty);
            createdCustomer.FullName.ShouldBe(customerRequest.FullName);
            createdCustomer.Email.ShouldBe(customerRequest.Email);
            createdCustomer.GitHubUsername.ShouldBe(customerRequest.GitHubUsername);
            createdCustomer.DateOfBirth.ShouldBe(customerRequest.DateOfBirth);
            // Store the created customer ID for cleanup
            _createdIds.Add(createdCustomer.Id);
        }
        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            // Cleanup created customers
            foreach (var id in _createdIds)
            {
                await _client.DeleteAsync($"/customers/{id}");
            }
        }
    }
}
