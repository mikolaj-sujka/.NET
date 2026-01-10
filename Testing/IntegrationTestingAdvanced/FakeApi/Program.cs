using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

var wireMockServer = WireMockServer.Start();

Console.WriteLine("Fake API server running at {0}", wireMockServer.Urls[0]);

wireMockServer.Given(Request.Create()
    .WithPath("/users/chaspas")
    .UsingGet()
    ).RespondWith(Response.Create()
    .WithStatusCode(200)
    .WithHeader("Content-Type", "application/json")
    .WithBody("{ \"message\": \"This is a fake API response\" }")
    );

Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();
wireMockServer.Stop();
