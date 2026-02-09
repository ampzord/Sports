using ErrorOr;
using Sports.Api.Database;
using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.GetMatchById;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Api.UnitTests.Infrastructure;
using Sports.Shared.Entities;

namespace Sports.Api.UnitTests.Handlers;

public class MatchHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly MatchMapper _mapper = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentLeague_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(999, 1, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("League.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(1, 999, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("HomeTeam.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(1, 1, 999, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("AwayTeam.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Leagues.Add(new League { Id = 2, Name = "La Liga" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Barcelona", LeagueId = 2 });
        await _db.SaveChangesAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(1, 1, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.DifferentLeagues");
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task GivenTeamsNotInSpecifiedLeague_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Leagues.Add(new League { Id = 2, Name = "La Liga" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(2, 1, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.LeagueMismatch");
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetMatchById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetMatchByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetMatchByIdQuery(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(999, 1, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        _db.Matches.Add(new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 });
        await _db.SaveChangesAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(1, 999, 2, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("HomeTeam.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        _db.Matches.Add(new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 });
        await _db.SaveChangesAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(1, 1, 999, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("AwayTeam.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenUpdateMatch_ThenReturnsValidationError()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Leagues.Add(new League { Id = 2, Name = "La Liga" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 2, Name = "Chelsea", LeagueId = 1 });
        _db.Teams.Add(new Team { Id = 3, Name = "Barcelona", LeagueId = 2 });
        _db.Matches.Add(new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 });
        await _db.SaveChangesAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(1, 1, 3, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.DifferentLeagues");
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenDeleteMatch_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteMatchHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteMatchCommand(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Match.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}
