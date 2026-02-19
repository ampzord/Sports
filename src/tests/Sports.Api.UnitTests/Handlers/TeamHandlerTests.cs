namespace Sports.Api.UnitTests.Handlers;

using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeamById;
using Sports.Api.Features.Teams.UpdateTeam;
using Sports.Api.UnitTests.Infrastructure;

public class TeamHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly TeamMapper _mapper = new();
    private static readonly Guid _nonExistentId = Guid.Empty;

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentLeague_WhenAddTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new AddTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddTeamCommand("Arsenal", _nonExistentId), CancellationToken.None);

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
            new AddTeamCommand("Arsenal", TestDataBuilder.Id(1)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.NameConflict);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenGetTeamById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetTeamByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetTeamByIdQuery(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.NotFound);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenUpdateTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateTeamHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateTeamCommand(_nonExistentId, "New Name", TestDataBuilder.Id(1)), CancellationToken.None);

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
            new UpdateTeamCommand(TestDataBuilder.Id(2), "Arsenal", TestDataBuilder.Id(1)), CancellationToken.None);

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
            new UpdateTeamCommand(TestDataBuilder.Id(1), "Arsenal", _nonExistentId), CancellationToken.None);

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
            new UpdateTeamCommand(TestDataBuilder.Id(1), "Arsenal", TestDataBuilder.Id(2)), CancellationToken.None);

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
            new UpdateTeamCommand(TestDataBuilder.Id(1), "Arsenal", TestDataBuilder.Id(2)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.LeagueId.Should().Be(TestDataBuilder.Id(2));
    }

    [Fact]
    public async Task Given_nonExistentId_WhenDeleteTeam_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteTeamHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteTeamCommand(_nonExistentId), CancellationToken.None);

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
            new DeleteTeamCommand(TestDataBuilder.Id(1)), CancellationToken.None);

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
            new DeleteTeamCommand(TestDataBuilder.Id(1)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.HasMatches);
    }
}
