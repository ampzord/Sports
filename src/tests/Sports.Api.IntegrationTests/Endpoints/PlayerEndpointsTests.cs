namespace Sports.Api.IntegrationTests.Endpoints;

using Sports.Api.Features.Players._Shared;
using Sports.Api.IntegrationTests.Infrastructure;
using Sports.Domain.Entities;
using Sports.Tests.Shared;
using System.Net.Http.Json;

public class PlayerEndpointsTests(SportsApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GivenValidRequest_WhenAddPlayer_ThenReturnsCreated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");

        // Act
        var response = await Client.PostAsJsonAsync(
            $"/{ApiRoutes.Prefix}/players",
            new { TeamId = team.Id, Name = "Bukayo Saka", Position = "RW" });

        // Assert
        response.Should().Be201Created();

        var player = await response.Content.ReadFromJsonAsync<PlayerResponse>(ApiHelper.JsonOptions);
        player.Should().NotBeNull();
        player!.Name.Should().Be("Bukayo Saka");
        player.Position.Should().Be(PlayerPosition.RW);
        player.TeamId.Should().Be(team.Id);
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenGetById_ThenReturnsPlayer()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var created = await Client.CreatePlayerAsync(team.Id, "Declan Rice", PlayerPosition.CDM);

        // Act
        var response = await Client.GetAsync($"/{ApiRoutes.Prefix}/players/{created.Id}");

        // Assert
        response.Should().Be200Ok();
        var player = await response.Content.ReadFromJsonAsync<PlayerResponse>(ApiHelper.JsonOptions);
        player!.Name.Should().Be("Declan Rice");
    }

    [Fact]
    public async Task GivenPlayersInTeam_WhenGetByTeamId_ThenReturnsFilteredPlayers()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");
        await Client.CreatePlayerAsync(team.Id, "Martin Odegaard", PlayerPosition.CAM);
        await Client.CreatePlayerAsync(team.Id, "William Saliba", PlayerPosition.CB);

        // Act
        var response = await Client.GetAsync($"/{ApiRoutes.Prefix}/players?teamId={team.Id}");

        // Assert
        response.Should().Be200Ok();
        var players = await response.Content.ReadFromJsonAsync<List<PlayerResponse>>(ApiHelper.JsonOptions);
        players.Should().HaveCount(2);
        players!.Should().AllSatisfy(p => p.TeamId.Should().Be(team.Id));
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenUpdate_ThenReturnsUpdated()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var created = await Client.CreatePlayerAsync(team.Id, "Original Player", PlayerPosition.LB);

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/{ApiRoutes.Prefix}/players/{created.Id}",
            new { Name = "Updated Player", Position = "RB", TeamId = team.Id });

        // Assert
        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<PlayerResponse>(ApiHelper.JsonOptions);
        updated!.Name.Should().Be("Updated Player");
        updated.Position.Should().Be(PlayerPosition.RB);
    }

    [Fact]
    public async Task GivenExistingPlayer_WhenDelete_ThenReturnsNoContent()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Premier League");
        var team = await Client.CreateTeamAsync(league.Id, "Arsenal");
        var created = await Client.CreatePlayerAsync(team.Id, "Aaron Ramsdale", PlayerPosition.GK);

        // Act
        var response = await Client.DeleteAsync($"/{ApiRoutes.Prefix}/players/{created.Id}");

        // Assert
        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/{ApiRoutes.Prefix}/players/99999");

        // Assert
        response.Should().Be404NotFound();
    }
}
