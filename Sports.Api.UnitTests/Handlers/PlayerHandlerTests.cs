using ErrorOr;
using Sports.Api.Database;
using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.DeletePlayer;
using Sports.Api.Features.Players.GetPlayerById;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Api.UnitTests.Infrastructure;
using Sports.Shared.Entities;

namespace Sports.Api.UnitTests.Handlers;

public class PlayerHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly PlayerMapper _mapper = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentTeam_WhenAddPlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new AddPlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddPlayerCommand("Saka", PlayerPosition.RW, 999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Team.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenAddPlayer_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Players.Add(new Player { Id = 1, Name = "Saka", Position = PlayerPosition.RW, TeamId = 1 });
        await _db.SaveChangesAsync();

        var handler = new AddPlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddPlayerCommand("Saka", PlayerPosition.LW, 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Player.NameConflict");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetPlayerById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetPlayerByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetPlayerByIdQuery(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Player.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenUpdatePlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdatePlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdatePlayerCommand(999, "Saka", PlayerPosition.RW, 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Player.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdatePlayer_ThenReturnsConflict()
    {
        // Arrange
        _db.Leagues.Add(new League { Id = 1, Name = "Premier League" });
        _db.Teams.Add(new Team { Id = 1, Name = "Arsenal", LeagueId = 1 });
        _db.Players.Add(new Player { Id = 1, Name = "Saka", Position = PlayerPosition.RW, TeamId = 1 });
        _db.Players.Add(new Player { Id = 2, Name = "Odegaard", Position = PlayerPosition.CAM, TeamId = 1 });
        await _db.SaveChangesAsync();

        var handler = new UpdatePlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdatePlayerCommand(2, "Saka", PlayerPosition.CAM, 1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Player.NameConflict");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenDeletePlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeletePlayerHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeletePlayerCommand(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Player.NotFound");
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}
