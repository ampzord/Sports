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
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam("Arsenal")
            .WithTeam("Chelsea")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(_nonExistentId, data.Id("Arsenal"), data.Id("Chelsea"), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam("Chelsea")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(data.Id("Premier League"), _nonExistentId, data.Id("Chelsea"), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.HomeTeamNotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenAddMatch_ThenReturnsNotFound()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(data.Id("Premier League"), data.Id("Arsenal"), _nonExistentId, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.AwayTeamNotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague("Premier League")
            .WithLeague("La Liga")
            .WithTeam("Arsenal")
            .WithTeam("Barcelona", inLeague: "La Liga")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(data.Id("Premier League"), data.Id("Arsenal"), data.Id("Barcelona"), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.DifferentLeagues);
    }

    [Fact]
    public async Task GivenTeamsNotInSpecifiedLeague_WhenAddMatch_ThenReturnsValidationError()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague("Premier League")
            .WithLeague("La Liga")
            .WithTeam("Arsenal")
            .WithTeam("Chelsea")
            .SaveAsync();

        var handler = new AddMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddMatchCommand(data.Id("La Liga"), data.Id("Arsenal"), data.Id("Chelsea"), null), CancellationToken.None);

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
            new UpdateMatchCommand(_nonExistentId, Guid.Empty, Guid.Empty, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentHomeTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam("Arsenal")
            .WithTeam("Chelsea")
            .WithMatch(homeTeam: "Arsenal", awayTeam: "Chelsea")
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(data.Id("match"), _nonExistentId, data.Id("Chelsea"), null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.HomeTeamNotFound);
    }

    [Fact]
    public async Task GivenNonExistentAwayTeam_WhenUpdateMatch_ThenReturnsNotFound()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam("Arsenal")
            .WithTeam("Chelsea")
            .WithMatch(homeTeam: "Arsenal", awayTeam: "Chelsea")
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(data.Id("match"), data.Id("Arsenal"), _nonExistentId, null), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(MatchErrors.AwayTeamNotFound);
    }

    [Fact]
    public async Task GivenTeamsFromDifferentLeagues_WhenUpdateMatch_ThenReturnsValidationError()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague("Premier League")
            .WithLeague("La Liga")
            .WithTeam("Arsenal")
            .WithTeam("Chelsea")
            .WithTeam("Barcelona", inLeague: "La Liga")
            .WithMatch(homeTeam: "Arsenal", awayTeam: "Chelsea")
            .SaveAsync();

        var handler = new UpdateMatchHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateMatchCommand(data.Id("match"), data.Id("Arsenal"), data.Id("Barcelona"), null), CancellationToken.None);

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
