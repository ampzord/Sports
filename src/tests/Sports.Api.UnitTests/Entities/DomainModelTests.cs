using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.UnitTests.Infrastructure;
using Sports.Shared.Entities;

namespace Sports.Api.UnitTests.Entities;

[Collection("Database")]
public class DomainModelTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private SportsDbContext _db = null!;

    public async Task InitializeAsync()
    {
        await fixture.ResetAsync();
        _db = fixture.CreateContext();
    }

    public Task DisposeAsync()
    {
        _db?.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GivenDuplicateLeagueName_WhenSaved_ThenThrowsException()
    {
        // Arrange
        _db.Leagues.Add(new League { Name = "Premier League" });
        await _db.SaveChangesAsync();

        _db.Leagues.Add(new League { Name = "Premier League" });

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task GivenDuplicateTeamName_WhenSaved_ThenThrowsException()
    {
        // Arrange
        var league = new League { Name = "La Liga" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        _db.Teams.Add(new Team { Name = "Barcelona", LeagueId = league.Id });
        await _db.SaveChangesAsync();

        _db.Teams.Add(new Team { Name = "Barcelona", LeagueId = league.Id });

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task GivenDuplicatePlayerName_WhenSaved_ThenThrowsException()
    {
        // Arrange
        var league = new League { Name = "Serie A" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        var team = new Team { Name = "AC Milan", LeagueId = league.Id };
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();

        _db.Players.Add(new Player { Name = "Theo", Position = PlayerPosition.LB, TeamId = team.Id });
        await _db.SaveChangesAsync();

        _db.Players.Add(new Player { Name = "Theo", Position = PlayerPosition.RB, TeamId = team.Id });

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task GivenLeagueWithTeams_WhenDeleteLeague_ThenThrowsException()
    {
        // Arrange
        var league = new League { Name = "Bundesliga" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        _db.Teams.Add(new Team { Name = "Bayern Munich", LeagueId = league.Id });
        await _db.SaveChangesAsync();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _db.Leagues.Remove(league));
    }

    [Fact]
    public async Task GivenTeamWithPlayers_WhenDeleteTeam_ThenThrowsException()
    {
        // Arrange
        var league = new League { Name = "Ligue 1" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        var team = new Team { Name = "PSG", LeagueId = league.Id };
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();

        _db.Players.Add(new Player { Name = "Mbappe", Position = PlayerPosition.ST, TeamId = team.Id });
        await _db.SaveChangesAsync();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _db.Teams.Remove(team));
    }

    [Fact]
    public async Task GivenTeamWithMatch_WhenDeleteTeam_ThenThrowsDbException()
    {
        // Arrange
        var league = new League { Name = "Eredivisie" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        var team1 = new Team { Name = "Ajax", LeagueId = league.Id };
        var team2 = new Team { Name = "Feyenoord", LeagueId = league.Id };
        _db.Teams.AddRange(team1, team2);
        await _db.SaveChangesAsync();

        _db.Matches.Add(new Match { HomeTeamId = team1.Id, AwayTeamId = team2.Id });
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();
        var freshTeam = await _db.Teams.FindAsync(team1.Id);
        _db.Teams.Remove(freshTeam!);

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task GivenPlayer_WhenTransferred_ThenTeamIdUpdated()
    {
        // Arrange
        var league = new League { Name = "Premier League" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        var arsenal = new Team { Name = "Arsenal", LeagueId = league.Id };
        var chelsea = new Team { Name = "Chelsea", LeagueId = league.Id };
        _db.Teams.AddRange(arsenal, chelsea);
        await _db.SaveChangesAsync();

        var player = new Player { Name = "Saka", Position = PlayerPosition.RW, TeamId = arsenal.Id };
        _db.Players.Add(player);
        await _db.SaveChangesAsync();

        // Act
        player.TeamId = chelsea.Id;
        await _db.SaveChangesAsync();

        // Assert
        _db.ChangeTracker.Clear();
        var transferred = await _db.Players.FindAsync(player.Id);
        transferred!.TeamId.Should().Be(chelsea.Id);
    }

    [Fact]
    public async Task GivenLeagueNameExceedingMaxLength_WhenSaved_ThenThrowsException()
    {
        // Arrange
        _db.Leagues.Add(new League { Name = new string('A', 101) });

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task GivenPositionEnum_WhenSavedAndReloaded_ThenStoredAsString()
    {
        // Arrange
        var league = new League { Name = "Test League" };
        _db.Leagues.Add(league);
        await _db.SaveChangesAsync();

        var team = new Team { Name = "Test Team", LeagueId = league.Id };
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();

        _db.Players.Add(new Player { Name = "Test GK", Position = PlayerPosition.GK, TeamId = team.Id });
        await _db.SaveChangesAsync();

        // Act
        var rawPosition = await _db.Database
            .SqlQueryRaw<string>("SELECT Position AS Value FROM Players WHERE Name = 'Test GK'")
            .FirstAsync();

        // Assert
        rawPosition.Should().Be("GK");
    }
}
