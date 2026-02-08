using System.Net.Http.Json;
using Sports.Api.Features.Leagues._Shared.Responses;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Matches._Shared.Responses;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Teams._Shared.Responses;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.IntegrationTests.Infrastructure;

namespace Sports.Api.IntegrationTests.Endpoints;

[Collection("Api")]
public class MatchEndpointsTests(SportsApiFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GivenValidRequest_WhenAddMatch_ThenReturnsCreated()
    {
        var (league, home, away) = await CreateMatchPrerequisitesAsync();

        var response = await _client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(league.Id, home.Id, away.Id));

        response.Should().Be201Created();

        var match = await response.Content.ReadFromJsonAsync<MatchResponse>();
        match.Should().NotBeNull();
        match!.HomeTeamId.Should().Be(home.Id);
        match.AwayTeamId.Should().Be(away.Id);
        match.TotalPasses.Should().BeNull();
    }

    [Fact]
    public async Task GivenExistingMatch_WhenGetById_ThenReturnsMatch()
    {
        var (league, home, away) = await CreateMatchPrerequisitesAsync();
        var created = await CreateMatchAsync(league.Id, home.Id, away.Id);

        var response = await _client.GetAsync($"/api/matches/{created.Id}");

        response.Should().Be200Ok();
        var match = await response.Content.ReadFromJsonAsync<MatchResponse>();
        match!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GivenMatchesInLeague_WhenGetByLeagueId_ThenReturnsFilteredMatches()
    {
        var (league, home, away) = await CreateMatchPrerequisitesAsync();
        await CreateMatchAsync(league.Id, home.Id, away.Id);

        var response = await _client.GetAsync($"/api/matches?leagueId={league.Id}");

        response.Should().Be200Ok();
        var matches = await response.Content.ReadFromJsonAsync<List<MatchResponse>>();
        matches.Should().HaveCount(1);
    }

    [Fact]
    public async Task GivenExistingMatch_WhenUpdate_ThenReturnsUpdated()
    {
        var (league, home, away) = await CreateMatchPrerequisitesAsync();
        var created = await CreateMatchAsync(league.Id, home.Id, away.Id);

        var response = await _client.PutAsJsonAsync(
            $"/api/matches/{created.Id}",
            new { HomeTeamId = away.Id, AwayTeamId = home.Id, TotalPasses = 500 });

        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<MatchResponse>();
        updated!.HomeTeamId.Should().Be(away.Id);
        updated.AwayTeamId.Should().Be(home.Id);
        updated.TotalPasses.Should().Be(500);
    }

    [Fact]
    public async Task GivenExistingMatch_WhenDelete_ThenReturnsNoContent()
    {
        var (league, home, away) = await CreateMatchPrerequisitesAsync();
        var created = await CreateMatchAsync(league.Id, home.Id, away.Id);

        var response = await _client.DeleteAsync($"/api/matches/{created.Id}");

        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/matches/99999");

        response.Should().Be404NotFound();
    }

    private async Task<(LeagueResponse League, TeamResponse Home, TeamResponse Away)> CreateMatchPrerequisitesAsync()
    {
        var leagueResponse = await _client.PostAsJsonAsync(
            "/api/leagues", new AddLeagueRequest("Premier League"));
        var league = (await leagueResponse.Content.ReadFromJsonAsync<LeagueResponse>())!;

        var homeResponse = await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Arsenal"));
        var home = (await homeResponse.Content.ReadFromJsonAsync<TeamResponse>())!;

        var awayResponse = await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Chelsea"));
        var away = (await awayResponse.Content.ReadFromJsonAsync<TeamResponse>())!;

        return (league, home, away);
    }

    private async Task<MatchResponse> CreateMatchAsync(int leagueId, int homeTeamId, int awayTeamId)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(leagueId, homeTeamId, awayTeamId));
        return (await response.Content.ReadFromJsonAsync<MatchResponse>())!;
    }
}
