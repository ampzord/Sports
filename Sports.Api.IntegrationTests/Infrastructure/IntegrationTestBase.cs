namespace Sports.Api.IntegrationTests.Infrastructure;

[Collection("Api")]
public abstract class IntegrationTestBase(SportsApiFactory factory) : IAsyncLifetime
{
    protected HttpClient Client { get; } = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;
}
