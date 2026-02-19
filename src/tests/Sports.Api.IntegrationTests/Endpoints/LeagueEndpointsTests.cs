namespace Sports.Api.IntegrationTests.Endpoints;

using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.IntegrationTests.Infrastructure;
using Sports.Tests.Shared;
using System.Net.Http.Json;

public class LeagueEndpointsTests(SportsApiFactory factory) : IntegrationTestBase(factory)
{

    [Fact]
    public async Task GivenNoLeagues_WhenGetAll_ThenReturnsEmptyList()
    {
        // Act
        var leagues = await Client.GetFromJsonAsync<List<LeagueResponse>>("leagues");

        // Assert
        leagues.Should().BeEmpty();
    }

    [Fact]
    public async Task GivenValidRequest_WhenAddLeague_ThenReturnsCreated()
    {
        // Act
        var response = await Client.PostAsJsonAsync(
            "leagues", new AddLeagueRequest("Premier League"));

        // Assert
        response.Should().Be201Created();

        var league = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        league.Should().NotBeNull();
        league!.Name.Should().Be("Premier League");
    }

    [Fact]
    public async Task GivenExistingLeague_WhenGetById_ThenReturnsLeague()
    {
        // Arrange
        var created = await Client.CreateLeagueAsync("La Liga");

        // Act
        var response = await Client.GetAsync($"leagues/{created.Id}");

        // Assert
        response.Should().Be200Ok();
        var league = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        league!.Name.Should().Be("La Liga");
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"leagues/{Guid.Empty}");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task GivenExistingLeague_WhenUpdate_ThenReturnsUpdated()
    {
        // Arrange
        var created = await Client.CreateLeagueAsync("Bundesliga");

        // Act
        var response = await Client.PutAsJsonAsync(
            $"leagues/{created.Id}", new { Name = "Ligue 1" });

        // Assert
        response.Should().Be200Ok();
        var updated = await response.Content.ReadFromJsonAsync<LeagueResponse>();
        updated!.Name.Should().Be("Ligue 1");
    }

    [Fact]
    public async Task GivenExistingLeague_WhenDelete_ThenReturnsNoContent()
    {
        // Arrange
        var created = await Client.CreateLeagueAsync("Eredivisie");

        // Act
        var response = await Client.DeleteAsync($"leagues/{created.Id}");

        // Assert
        response.Should().Be204NoContent();
    }

    [Fact]
    public async Task GivenLeagueWithTeams_WhenDelete_ThenReturnsConflict()
    {
        // Arrange
        var league = await Client.CreateLeagueAsync("Serie A");
        await Client.CreateTeamAsync(league.Id, "AC Milan");

        // Act
        var response = await Client.DeleteAsync($"leagues/{league.Id}");

        // Assert
        response.Should().Be409Conflict();
    }
}
