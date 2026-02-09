using System.Net.Http.Json;
using Sports.Api.Features.Teams._Shared.Responses;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.IntegrationTests.Infrastructure;
using Sports.Tests.Shared;

namespace Sports.Api.IntegrationTests.Endpoints;

public class TeamEndpointsTests(SportsApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GivenValidRequest_WhenAddTeam_ThenReturnsCreated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/teams", new AddTeamRequest(league.Id, "Arsenal"));

        // Assert
        response.Should().Be201Created();

        var team = await response.Content.ReadFromJsonAsync<TeamResponse>();
        team.Should().NotBeNull();
        team!.Name.Should().Be("Arsenal");
        team.LeagueId.Should().Be(league.Id);
    }

    [Fact]
    public async Task GivenExistingTeam_WhenGetById_ThenReturnsTeam()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("La Liga");
        var created = await Client.CreateTeamAsync(league.Id, "Barcelona");

        // Act
        var response = await Client.GetAsync($"/api/teams/{created.Id}");

        // Assert
        response.Should().Be200Ok();
        var team = await response.Content.ReadFromJsonAsync<TeamResponse>();
        team!.Name.Should().Be("Barcelona");
    }

    [Fact]
    public async Task GivenTeamsInLeague_WhenGetByLeagueId_ThenReturnsFilteredTeams()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Serie A");
        await Client.CreateTeamAsync(league.Id, "AC Milan");
        await Client.CreateTeamAsync(league.Id, "Inter Milan");

        // Act
        var response = await Client.GetAsync($"/api/teams?leagueId={league.Id}");

        // Assert
        response.Should().Be200Ok();
        var teams = await response.Content.ReadFromJsonAsync<List<TeamResponse>>();
        teams.Should().HaveCount(2);
        teams!.Should().AllSatisfy(t => t.LeagueId.Should().Be(league.Id));
    }

    [Fact]
    public async Task GivenExistingTeam_WhenUpdate_ThenReturnsUpdated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Bundesliga");
        var created = await Client.CreateTeamAsync(league.Id, "Original Team");

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/api/teams/{created.Id}",
            new { Name = "Updated Team", LeagueId = league.Id });

        // Assert
        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<TeamResponse>();
        updated!.Name.Should().Be("Updated Team");
    }

    [Fact]
    public async Task GivenExistingTeam_WhenDelete_ThenReturnsNoContent()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Ligue 1");
        var created = await Client.CreateTeamAsync(league.Id, "PSG");

        // Act
        var response = await Client.DeleteAsync($"/api/teams/{created.Id}");

        // Assert
        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/teams/99999");

        // Assert
        response.Should().Be404NotFound();
    }
}
