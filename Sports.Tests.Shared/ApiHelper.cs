namespace Sports.Tests.Shared;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sports.Api.Features.Leagues._Shared.Responses;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Matches._Shared.Responses;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Players._Shared.Responses;
using Sports.Api.Features.Teams._Shared.Responses;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Shared.Entities;

public static class ApiHelper
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<LeagueResponse> CreateLeagueAsync(this HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/api/leagues", new AddLeagueRequest(name));
        return (await response.Content.ReadFromJsonAsync<LeagueResponse>())!;
    }

    public static async Task<TeamResponse> CreateTeamAsync(this HttpClient client, int leagueId, string name)
    {
        var response = await client.PostAsJsonAsync("/api/teams", new AddTeamRequest(leagueId, name));
        return (await response.Content.ReadFromJsonAsync<TeamResponse>())!;
    }

    public static async Task<PlayerResponse> CreatePlayerAsync(
        this HttpClient client, int teamId, string name, PlayerPosition position)
    {
        var response = await client.PostAsJsonAsync(
            "/api/players",
            new { TeamId = teamId, Name = name, Position = position.ToString() });
        return (await response.Content.ReadFromJsonAsync<PlayerResponse>(JsonOptions))!;
    }

    public static async Task<MatchResponse> CreateMatchAsync(
        this HttpClient client, int leagueId, int homeTeamId, int awayTeamId)
    {
        var response = await client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(leagueId, homeTeamId, awayTeamId));
        return (await response.Content.ReadFromJsonAsync<MatchResponse>())!;
    }
}
