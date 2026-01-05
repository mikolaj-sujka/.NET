using Bogus;
using Customers.Api.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.Tests.Integration.CustomerController
{
    [Collection("IntegrationTestCollection")]
    public class GetCustomerControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        private readonly HttpClient _client = appFactory.CreateClient();

        private readonly Faker<CustomerRequest> _costumerGenerator =
            new Faker<CustomerRequest>()
                .RuleFor(x => x.FullName, faker => faker.Person.FullName)
                .RuleFor(x => x.Email, faker => faker.Person.Email)
                .RuleFor(x => x.GitHubUsername, "nickchapsas")
                .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

        [Fact]
        public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync($"/customers/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var text = await response.Content.ReadAsStringAsync();
            text.ShouldContain("404");

            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem!.Status.ShouldBe(404);
        }
    }
}
