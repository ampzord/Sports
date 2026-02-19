namespace Sports.Api.UnitTests.Handlers;

using Sports.Api.Database;
using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.DeletePlayer;
using Sports.Api.Features.Players.GetPlayerById;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Api.Features.Teams._Shared;
using Sports.Api.UnitTests.Infrastructure;
using Sports.Domain.Entities;

public class PlayerHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly PlayerMapper _mapper = new();
    private static readonly Guid _nonExistentId = Guid.Empty;

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenNonExistentTeam_WhenAddPlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new AddPlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddPlayerCommand("Saka", PlayerPosition.RW, _nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(TeamErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenAddPlayer_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .WithPlayer()
            .SaveAsync();

        var handler = new AddPlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddPlayerCommand("Saka", PlayerPosition.LW, TestDataBuilder.Id(1)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(PlayerErrors.NameConflict);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenGetPlayerById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetPlayerByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetPlayerByIdQuery(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(PlayerErrors.NotFound);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenUpdatePlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdatePlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdatePlayerCommand(_nonExistentId, "Saka", PlayerPosition.RW, TestDataBuilder.Id(1)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(PlayerErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdatePlayer_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .WithPlayer(1, "Saka")
            .WithPlayer(2, "Odegaard", PlayerPosition.CAM)
            .SaveAsync();

        var handler = new UpdatePlayerHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdatePlayerCommand(TestDataBuilder.Id(2), "Saka", PlayerPosition.CAM, TestDataBuilder.Id(1)), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(PlayerErrors.NameConflict);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenDeletePlayer_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeletePlayerHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeletePlayerCommand(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(PlayerErrors.NotFound);
    }
}
