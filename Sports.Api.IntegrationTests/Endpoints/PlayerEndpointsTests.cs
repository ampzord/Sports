using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sports.Api.Features.Leagues._Shared.Responses;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Players._Shared.Responses;
using Sports.Api.Features.Teams._Shared.Responses;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.IntegrationTests.Infrastructure;
using Sports.Shared.Entities;

namespace Sports.Api.IntegrationTests.Endpoints;

[Collection("Api")]
public class PlayerEndpointsTests(SportsApiFactory factory) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GivenValidRequest_WhenAddPlayer_ThenReturnsCreated()
    {
        var team = await CreateTeamWithLeagueAsync();

        var response = await _client.PostAsJsonAsync(
            "/api/players",
            new { TeamId = team.Id, Name = "Bukayo Saka", Position = "RW" });

        response.Should().Be201Created();

        var player = await response.Content.ReadFromJsonAsync<PlayerResponse>(JsonOptions);
        player.Should().NotBeNull();
        player!.Name.Should().Be("Bukayo Saka");
        player.Position.Should().Be(PlayerPosition.RW);
        player.TeamId.Should().Be(team.Id);
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenGetById_ThenReturnsPlayer()
    {
        var team = await CreateTeamWithLeagueAsync();
        var created = await CreatePlayerAsync(team.Id, "Declan Rice", PlayerPosition.CDM);

        var response = await _client.GetAsync($"/api/players/{created.Id}");

        response.Should().Be200Ok();
        var player = await response.Content.ReadFromJsonAsync<PlayerResponse>(JsonOptions);
        player!.Name.Should().Be("Declan Rice");
    }

    [Fact]
    public async Task GivenPlayersInTeam_WhenGetByTeamId_ThenReturnsFilteredPlayers()
    {
        var team = await CreateTeamWithLeagueAsync();
        await CreatePlayerAsync(team.Id, "Martin Odegaard", PlayerPosition.CAM);
        await CreatePlayerAsync(team.Id, "William Saliba", PlayerPosition.CB);

        var response = await _client.GetAsync($"/api/players?teamId={team.Id}");

        response.Should().Be200Ok();
        var players = await response.Content.ReadFromJsonAsync<List<PlayerResponse>>(JsonOptions);
        players.Should().HaveCount(2);
        players!.Should().AllSatisfy(p => p.TeamId.Should().Be(team.Id));
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenUpdate_ThenReturnsUpdated()
    {
        var team = await CreateTeamWithLeagueAsync();
        var created = await CreatePlayerAsync(team.Id, "Original Player", PlayerPosition.LB);

        var response = await _client.PutAsJsonAsync(
            $"/api/players/{created.Id}",
            new { Name = "Updated Player", Position = "RB", TeamId = team.Id });

        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<PlayerResponse>(JsonOptions);
        updated!.Name.Should().Be("Updated Player");
        updated.Position.Should().Be(PlayerPosition.RB);
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenDelete_ThenReturnsNoContent()
    {
        var team = await CreateTeamWithLeagueAsync();
        var created = await CreatePlayerAsync(team.Id, "To Delete Player", PlayerPosition.GK);

        var response = await _client.DeleteAsync($"/api/players/{created.Id}");

        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/players/99999");

        response.Should().Be404NotFound();
    }

    private async Task<TeamResponse> CreateTeamWithLeagueAsync()
    {
        var leagueResponse = await _client.PostAsJsonAsync(
            "/api/leagues", new AddLeagueRequest("Premier League"));
        var league = (await leagueResponse.Content.ReadFromJsonAsync<LeagueResponse>())!;

        var teamResponse = await _client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Arsenal"));
        return (await teamResponse.Content.ReadFromJsonAsync<TeamResponse>())!;
    }

    private async Task<PlayerResponse> CreatePlayerAsync(int teamId, string name, PlayerPosition position)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/players",
            new { TeamId = teamId, Name = name, Position = position.ToString() });
        return (await response.Content.ReadFromJsonAsync<PlayerResponse>(JsonOptions))!;
    }
}
