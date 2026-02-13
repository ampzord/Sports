using System.Net.Http.Json;
using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.IntegrationTests.Infrastructure;
using Sports.Tests.Shared;

namespace Sports.Api.IntegrationTests.Endpoints;

public class MatchEndpointsTests(SportsApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GivenValidRequest_WhenAddMatch_ThenReturnsCreated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var home = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var away = await Client.CreateTeamAsync(league.Id, "Chelsea");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(league.Id, home.Id, away.Id));

        // Assert
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
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var home = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var away = await Client.CreateTeamAsync(league.Id, "Chelsea");
        var created = await Client.CreateMatchAsync(league.Id, home.Id, away.Id);

        // Act
        var response = await Client.GetAsync($"/api/matches/{created.Id}");

        // Assert
        response.Should().Be200Ok();
        var match = await response.Content.ReadFromJsonAsync<MatchResponse>();
        match!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GivenMatchesInLeague_WhenGetByLeagueId_ThenReturnsFilteredMatches()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var home = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var away = await Client.CreateTeamAsync(league.Id, "Chelsea");
        await Client.CreateMatchAsync(league.Id, home.Id, away.Id);

        // Act
        var response = await Client.GetAsync($"/api/matches?leagueId={league.Id}");

        // Assert
        response.Should().Be200Ok();
        var matches = await response.Content.ReadFromJsonAsync<List<MatchResponse>>();
        matches.Should().HaveCount(1);
    }

    [Fact]
    public async Task GivenExistingMatch_WhenUpdate_ThenReturnsUpdated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var home = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var away = await Client.CreateTeamAsync(league.Id, "Chelsea");
        var created = await Client.CreateMatchAsync(league.Id, home.Id, away.Id);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/api/matches/{created.Id}",
            new { HomeTeamId = away.Id, AwayTeamId = home.Id, TotalPasses = 500 });

        // Assert
        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<MatchResponse>();
        updated!.HomeTeamId.Should().Be(away.Id);
        updated.AwayTeamId.Should().Be(home.Id);
        updated.TotalPasses.Should().Be(500);
    }

    [Fact]
    public async Task GivenExistingMatch_WhenDelete_ThenReturnsNoContent()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var home = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var away = await Client.CreateTeamAsync(league.Id, "Chelsea");
        var created = await Client.CreateMatchAsync(league.Id, home.Id, away.Id);

        // Act
        var response = await Client.DeleteAsync($"/api/matches/{created.Id}");

        // Assert
        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/matches/99999");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task GivenSameHomeAndAwayTeam_WhenAddMatch_ThenReturnsBadRequest()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(league.Id, team.Id, team.Id));

        // Assert
        response.Should().Be400BadRequest();
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenAddMatch_ThenReturnsBadRequest()
    {
        // Arrange
        var premierLeague = await Client.CreateLeagueAsync("Premier League");
        var laLiga = await Client.CreateLeagueAsync("La Liga");
        var arsenal = await Client.CreateTeamAsync(premierLeague.Id, "Arsenal");
        var barcelona = await Client.CreateTeamAsync(laLiga.Id, "Barcelona");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(premierLeague.Id, arsenal.Id, barcelona.Id));

        // Assert
        response.Should().Be400BadRequest();
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenUpdateMatch_ThenReturnsBadRequest()
    {
        // Arrange
        var premierLeague = await Client.CreateLeagueAsync("Premier League");
        var laLiga = await Client.CreateLeagueAsync("La Liga");
        var arsenal = await Client.CreateTeamAsync(premierLeague.Id, "Arsenal");
        var chelsea = await Client.CreateTeamAsync(premierLeague.Id, "Chelsea");
        var barcelona = await Client.CreateTeamAsync(laLiga.Id, "Barcelona");
        var created = await Client.CreateMatchAsync(premierLeague.Id, arsenal.Id, chelsea.Id);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/api/matches/{created.Id}",
            new { HomeTeamId = arsenal.Id, AwayTeamId = barcelona.Id });

        // Assert
        response.Should().Be400BadRequest();
    }

    [Fact]
    public async Task GivenTeamsNotInSpecifiedLeague_WhenAddMatch_ThenReturnsBadRequest()
    {
        // Arrange
        var premierLeague = await Client.CreateLeagueAsync("Premier League");
        var laLiga = await Client.CreateLeagueAsync("La Liga");
        var arsenal = await Client.CreateTeamAsync(premierLeague.Id, "Arsenal");
        var chelsea = await Client.CreateTeamAsync(premierLeague.Id, "Chelsea");

        // Act — teams are in the same league, but not the one specified
        var response = await Client.PostAsJsonAsync(
            "/api/matches", new AddMatchRequest(laLiga.Id, arsenal.Id, chelsea.Id));

        // Assert
        response.Should().Be400BadRequest();
    }
}
