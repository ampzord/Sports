namespace Sports.Api.UnitTests.Handlers;

using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.GetMatchById;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Api.UnitTests.Infrastructure;

public class MatchHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly MatchMapper _mapper = new();
    private static readonly Guid _nonExistentId = Guid.Empty;

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentLeague_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(1, "Arsenal")
            .WithTeam(2, "Chelsea")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(_nonExistentId, TestDataBuilder.Id(1), TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(2, "Chelsea")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(TestDataBuilder.Id(1), _nonExistentId, TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.HomeTeamNotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(TestDataBuilder.Id(1), TestDataBuilder.Id(1), _nonExistentId, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.AwayTeamNotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .WithTeam(1, "Arsenal", leagueId: 1)
            .WithTeam(2, "Barcelona", leagueId: 2)
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(TestDataBuilder.Id(1), TestDataBuilder.Id(1), TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.DifferentLeagues);
    }

    [Fact]
    public async Task GivenTeamsNotInSpecifiedLeague_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .WithTeam(1, "Arsenal", leagueId: 1)
            .WithTeam(2, "Chelsea", leagueId: 1)
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(TestDataBuilder.Id(2), TestDataBuilder.Id(1), TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.LeagueMismatch);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenGetMatchById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetMatchByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetMatchByIdQuery(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.NotFound);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(_nonExistentId, TestDataBuilder.Id(1), TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(1, "Arsenal")
            .WithTeam(2, "Chelsea")
            .WithMatch(1, homeTeamId: 1, awayTeamId: 2)
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(TestDataBuilder.Id(1), _nonExistentId, TestDataBuilder.Id(2), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.HomeTeamNotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(1, "Arsenal")
            .WithTeam(2, "Chelsea")
            .WithMatch(1, homeTeamId: 1, awayTeamId: 2)
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(TestDataBuilder.Id(1), TestDataBuilder.Id(1), _nonExistentId, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.AwayTeamNotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenUpdateMatch_ThenReturnsValidationError()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .WithTeam(1, "Arsenal", leagueId: 1)
            .WithTeam(2, "Chelsea", leagueId: 1)
            .WithTeam(3, "Barcelona", leagueId: 2)
            .WithMatch(1, homeTeamId: 1, awayTeamId: 2)
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(TestDataBuilder.Id(1), TestDataBuilder.Id(1), TestDataBuilder.Id(3), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.DifferentLeagues);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenDeleteMatch_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteMatchHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteMatchCommand(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.NotFound);
    }
}
