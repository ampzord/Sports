namespace Sports.Api.UnitTests.Handlers;

using ErrorOr;
using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.GetLeagueById;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Api.UnitTests.Infrastructure;

public class LeagueHandlerTests : IDisposable
{
    private readonly SportsDbContext _db = InMemoryDbContextFactory.Create();
    private readonly LeagueMapper _mapper = new();
    private static readonly Guid _nonExistentId = Guid.Empty;

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
    public async Task Given_nonExistentId_WhenGetLeagueById_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new GetLeagueByIdHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new GetLeagueByIdQuery(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenUpdateLeague_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new UpdateLeagueHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateLeagueCommand(_nonExistentId, "New Name"), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenDuplicateName_WhenUpdateLeague_ThenReturnsConflict()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague("Premier League")
            .WithLeague("La Liga")
            .SaveAsync();

        var handler = new UpdateLeagueHandler(_db, _mapper);

        // Act
        var result = await handler.Handle(
            new UpdateLeagueCommand(data.Id("La Liga"), "Premier League"), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NameConflict);
    }

    [Fact]
    public async Task Given_nonExistentId_WhenDeleteLeague_ThenReturnsNotFound()
    {
        // Arrange
        var handler = new DeleteLeagueHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteLeagueCommand(_nonExistentId), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.NotFound);
    }

    [Fact]
    public async Task GivenLeagueWithTeams_WhenDeleteLeague_ThenReturnsConflict()
    {
        // Arrange
        var data = await new TestDataBuilder(_db)
            .WithLeague()
            .WithTeam()
            .SaveAsync();

        var handler = new DeleteLeagueHandler(_db);

        // Act
        var result = await handler.Handle(
            new DeleteLeagueCommand(data.Id("Premier League")), CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(LeagueErrors.HasTeams);
    }
}
