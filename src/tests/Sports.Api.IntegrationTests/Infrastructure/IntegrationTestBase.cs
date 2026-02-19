namespace Sports.Api.IntegrationTests.Infrastructure;

using Sports.Api;

[Collection("Api")]
public abstract class IntegrationTestBase(SportsApiFactory factory) : IAsyncLifetime
{
    protected HttpClient Client { get; } = CreateClient(factory);

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private static HttpClient CreateClient(SportsApiFactory factory)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(client.BaseAddress!, $"/{ApiRoutes.Prefix}/");
        return client;
    }
}
