using ErrorOr;
using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.GetLeagueById;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Api.UnitTests.Infrastructure;

namespace Sports.Api.UnitTests.Handlers;

public class LeagueHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly LeagueMapper _mapper = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task GivenDuplicateName_WhenAddLeague_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .SaveAsync();

        var handler = new AddLeagueHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new AddLeagueCommand("Premier League"), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NameConflict);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenGetLeagueById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetLeagueByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetLeagueByIdQuery(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenUpdateLeague_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateLeagueHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateLeagueCommand(999, "New Name"), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdateLeague_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague(1, "Premier League")
            .WithLeague(2, "La Liga")
            .SaveAsync();

        var handler = new UpdateLeagueHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateLeagueCommand(2, "Premier League"), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NameConflict);
    }

    [Fact]
    public async Task GivenNonExistentId_WhenDeleteLeague_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteLeagueHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteLeagueCommand(999), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenLeagueWithTeams_WhenDeleteLeague_ThenReturnsConflict()
    {
        // Arrange
        await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new DeleteLeagueHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteLeagueCommand(1), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.HasTeams);
    }
}
