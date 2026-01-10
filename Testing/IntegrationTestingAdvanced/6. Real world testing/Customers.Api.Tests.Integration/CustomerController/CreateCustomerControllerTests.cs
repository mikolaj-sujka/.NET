using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Customers.Api.Tests.Integration.CustomerController;

public class CreateCustomerControllerTests(CustomerApiFactory apiFactory) : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client = apiFactory.CreateClient();

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(c => c.FullName, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Person.Email)
        .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ValidGitHubUsername)
        .RuleFor(c => c.DateOfBirth, f => f.Person.DateOfBirth.Date);

    [Fact]
    public async Task Test()
    {
        await Task.Delay(500);
    }

    [Fact]
    public async Task Create_CreateUser_WhenDataIsValid()
    {
        // Arrange
        var newCustomer = _customerGenerator.Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", newCustomer);
        
        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.GitHubUsername.ShouldBeEquivalentTo(newCustomer.GitHubUsername);
        customerResponse.FullName.ShouldBeEquivalentTo(newCustomer.FullName);
        customerResponse.Email.ShouldBeEquivalentTo(newCustomer.Email);
        customerResponse.DateOfBirth.ShouldBeEquivalentTo(newCustomer.DateOfBirth);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenGitHubUserDoesNotExist()
    {
        // Arrange
        var newCustomer = _customerGenerator
            .RuleFor(x => x.GitHubUsername, "nonexistinguser12345")
            .Generate();
        
        // Act
        var response = await _client.PostAsJsonAsync("customers", newCustomer);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error.Title.ShouldBe("One or more validation errors occurred.");
    }

}