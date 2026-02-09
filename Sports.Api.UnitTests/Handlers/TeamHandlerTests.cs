using ErrorOr;
using Sports.Api.Database;
using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeamById;
using Sports.Api.Features.Teams.UpdateTeam;
using Sports.Api.UnitTests.Infrastructure;
using Sports.Shared.Entities;

namespace Sports.Api.UnitTests.Handlers;

public class TeamHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly TeamMapper _mapper = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentLeague_WhenAddTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new AddTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddTeamCommand("Arsenal", 999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("League.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenAddTeam_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddTeamCommand("Arsenal", 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NameConflict");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetTeamById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetTeamByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetTeamByIdQuery(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenUpdateTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(999, "New Name", 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdateTeam_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(2, "Arsenal", 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NameConflict");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task GivenNonExistentLeague_WhenUpdateTeamLeague_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(1, "Arsenal", 999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("League.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenTeamWithMatches_WhenChangeLeague_ThenReturnsValidationError()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Leagues.Add(new League { Id = 2, Name = "La Liga" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        _db.Matches.Add(new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 });
        await _db.SaveChangesAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(1, "Arsenal", 2), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.HasMatches");
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task GivenTeamWithNoMatches_WhenChangeLeague_ThenSucceeds()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Leagues.Add(new League { Id = 2, Name = "La Liga" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(1, "Arsenal", 2), CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.LeagueId.Should().Be(2);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenDeleteTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenTeamWithPlayers_WhenDeleteTeam_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Players.Add(new Player { Id = 1, Name = "Saka", Position = PlayerPosition.RW, TeamId = 1 });
        await _db.SaveChangesAsync();

        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.HasPlayers");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task GivenTeamWithMatches_WhenDeleteTeam_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        _db.Matches.Add(new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 });
        await _db.SaveChangesAsync();

        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.HasMatches");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }
}
