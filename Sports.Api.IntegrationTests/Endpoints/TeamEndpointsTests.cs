using System.Net.Http.Json;
using Sports.Api.Features.Leagues._Shared.Responses;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Teams._Shared.Responses;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.IntegrationTests.Infrastructure;

namespace Sports.Api.IntegrationTests.Endpoints;

[Collection("Api")]
public class TeamEndpointsTests(SportsApiFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GivenValidRequest_WhenAddTeam_ThenReturnsCreated()
    {
        var league = await CreateLeagueAsync("Premier League");

        var response = await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Arsenal"));

        response.Should().Be201Created();

        var team = await response.Content.ReadFromJsonAsync<TeamResponse>();
        team.Should().NotBeNull();
        team!.Name.Should().Be("Arsenal");
        team.LeagueId.Should().Be(league.Id);
    }

    [Fact]
    public async Task GivenExistingTeam_WhenGetById_ThenReturnsTeam()
    {
        var league = await CreateLeagueAsync("La Liga");
        var created = await CreateTeamAsync(league.Id, "Barcelona");

        var response = await _client.GetAsync($"/api/teams/{created.Id}");

        response.Should().Be200Ok();
        var team = await response.Content.ReadFromJsonAsync<TeamResponse>();
        team!.Name.Should().Be("Barcelona");
    }

    [Fact]
    public async Task GivenTeamsInLeague_WhenGetByLeagueId_ThenReturnsFilteredTeams()
    {
        var league = await CreateLeagueAsync("Serie A");
        await CreateTeamAsync(league.Id, "AC Milan");
        await CreateTeamAsync(league.Id, "Inter Milan");

        var response = await _client.GetAsync($"/api/teams?leagueId={league.Id}");

        response.Should().Be200Ok();
        var teams = await response.Content.ReadFromJsonAsync<List<TeamResponse>>();
        teams.Should().HaveCount(2);
        teams!.Should().AllSatisfy(t => t.LeagueId.Should().Be(league.Id));
    }

    [Fact]
    public async Task GivenExistingTeam_WhenUpdate_ThenReturnsUpdated()
    {
        var league = await CreateLeagueAsync("Bundesliga");
        var created = await CreateTeamAsync(league.Id, "Original Team");

        var response = await _client.PutAsJsonAsync(
            $"/api/teams/{created.Id}",
            new { Name = "Updated Team", LeagueId = league.Id });

        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<TeamResponse>();
        updated!.Name.Should().Be("Updated Team");
    }

    [Fact]
    public async Task GivenExistingTeam_WhenDelete_ThenReturnsNoContent()
    {
        var league = await CreateLeagueAsync("Ligue 1");
        var created = await CreateTeamAsync(league.Id, "PSG");

        var response = await _client.DeleteAsync($"/api/teams/{created.Id}");

        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/teams/99999");

        response.Should().Be404NotFound();
    }

    private async Task<LeagueResponse> CreateLeagueAsync(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/leagues", new AddLeagueRequest(name));
        return (await response.Content.ReadFromJsonAsync<LeagueResponse>())!;
    }

    private async Task<TeamResponse> CreateTeamAsync(int leagueId, string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(leagueId, name));
        return (await response.Content.ReadFromJsonAsync<TeamResponse>())!;
    }
}
