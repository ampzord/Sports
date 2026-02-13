using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeamById;
using Sports.Api.Features.Teams.UpdateTeam;
using Sports.Api.UnitTests.Infrastructure;

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
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenAddTeam_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new AddTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddTeamCommand("Arsenal", 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.NameConflict);
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
        result.FirstError.Should().Be(TeamErrors.NotFound);
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
        result.FirstError.Should().Be(TeamErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdateTeam_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(1, "Arsenal")
            .WithTeam(2, "Chelsea")
            .SaveAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(2, "Arsenal", 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.NameConflict);
    }

    [Fact]
    public async Task GivenNonExistentLeague_WhenUpdateTeamLeague_ThenReturnsNotFound()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(1, "Arsenal", 999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenTeamWithMatches_WhenChangeLeague_ThenReturnsValidationError()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .WithTeam(1, "Arsenal", leagueId: 1)
            .WithTeam(2, "Chelsea", leagueId: 1)
            .WithMatch(1, homeTeamId: 1, awayTeamId: 2)
            .SaveAsync();

        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(1, "Arsenal", 2), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.HasMatches);
    }

    [Fact]
    public async Task GivenTeamWithNoMatches_WhenChangeLeague_ThenSucceeds()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .WithTeam()
            .SaveAsync();

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
        result.FirstError.Should().Be(TeamErrors.NotFound);
    }

    [Fact]
    public async Task GivenTeamWithPlayers_WhenDeleteTeam_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .WithPlayer()
            .SaveAsync();

        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.HasPlayers);
    }

    [Fact]
    public async Task GivenTeamWithMatches_WhenDeleteTeam_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam(1, "Arsenal")
            .WithTeam(2, "Chelsea")
            .WithMatch(1, homeTeamId: 1, awayTeamId: 2)
            .SaveAsync();

        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.HasMatches);
    }
}
