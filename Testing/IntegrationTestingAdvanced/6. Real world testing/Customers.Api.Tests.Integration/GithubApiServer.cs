using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customers.Api.Tests.Integration;

public class GithubApiServer
{
    private WireMockServer _server;
    public string Url => _server.Url!;

    public void Start()
    {
        _server = WireMockServer.Start(8081);
    }

    public void SetupUser(string username)
    {
        _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody($@"{{""login"": ""{username}""}}")
                .WithHeader("Content-Type", "application/json")
                .WithStatusCode(200));
    }

    public void Stop()
    {
        _server.Stop();
        _server.Dispose();
    }
}