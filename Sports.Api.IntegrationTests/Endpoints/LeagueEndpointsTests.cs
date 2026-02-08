using System.Net.Http.Json;
using Sports.Api.Features.Leagues._Shared.Responses;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.IntegrationTests.Infrastructure;

namespace Sports.Api.IntegrationTests.Endpoints;

[Collection("Api")]
public class LeagueEndpointsTests(SportsApiFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GivenNoLeagues_WhenGetAll_ThenReturnsEmptyList()
    {
        var leagues = await _client.GetFromJsonAsync<List<LeagueResponse>>("/api/leagues");

        leagues.Should().BeEmpty();
    }

    [Fact]
    public async Task GivenValidRequest_WhenAddLeague_ThenReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/leagues", new AddLeagueRequest("Premier League"));

        response.Should().Be201Created();

        var league = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        league.Should().NotBeNull();
        league!.Name.Should().Be("Premier League");
        league.Id.Should().Be(1);
    }

    [Fact]
    public async Task GivenExistingLeague_WhenGetById_ThenReturnsLeague()
    {
        var created = await CreateLeagueAsync("La Liga");

        var response = await _client.GetAsync($"/api/leagues/{created.Id}");

        response.Should().Be200Ok();
        var league = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        league!.Name.Should().Be("La Liga");
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/leagues/99999");

        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task GivenExistingLeague_WhenUpdate_ThenReturnsUpdated()
    {
        var created = await CreateLeagueAsync("Original League");

        var response = await _client.PutAsJsonAsync(
            $"/api/leagues/{created.Id}", new { Name = "Updated League" });

        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        updated!.Name.Should().Be("Updated League");
    }

    [Fact]
    public async Task GivenExistingLeague_WhenDelete_ThenReturnsNoContent()
    {
        var created = await CreateLeagueAsync("To Delete League");

        var response = await _client.DeleteAsync($"/api/leagues/{created.Id}");

        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenLeagueWithTeams_WhenDelete_ThenReturnsBadRequest()
    {
        var league = await CreateLeagueAsync("League With Teams");
        await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Team A"));

        var response = await _client.DeleteAsync($"/api/leagues/{league.Id}");

        response.Should().Be409Conflict();
    }

    private async Task<LeagueResponse> CreateLeagueAsync(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/leagues", new AddLeagueRequest(name));
        return (await response.Content.ReadFromJsonAsync<LeagueResponse>())!;
    }
}
