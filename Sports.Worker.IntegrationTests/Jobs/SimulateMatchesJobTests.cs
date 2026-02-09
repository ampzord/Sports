using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sports.MatchSimulationWorker.Jobs;
using Sports.Worker.IntegrationTests.Infrastructure;

namespace Sports.Worker.IntegrationTests.Jobs;

[Collection("Worker")]
public class SimulateMatchesJobTests(WorkerDatabaseFixture fixture)
{
    [Fact]
    public async Task GivenMatchesWithNullPasses_WhenSimulated_ThenAllMatchesGetTotalPasses()
    {
        // Arrange
        await fixture.SeedMatchesAsync(50);

        var db = fixture.CreateContext();
        var logger = new Mock<ILogger<SimulateMatchesJob>>();
        var job = new SimulateMatchesJob(db, logger.Object);

        // Act
        await job.SimulateMatchPassesAsync();

        // Assert
        db.ChangeTracker.Clear();
        var matches = await db.Matches.ToListAsync();
        matches.Should().HaveCount(50);
        matches.Should().AllSatisfy(m =>
        {
            m.TotalPasses.Should().NotBeNull();
            m.TotalPasses.Should().BeInRange(
                ISimulateMatchesJob.MinPasses,
                ISimulateMatchesJob.MaxPasses - 1);
        });
    }

    [Fact]
    public async Task GivenNoMatchesWithNullPasses_WhenSimulated_ThenCompletesImmediately()
    {
        // Arrange
        await fixture.SeedMatchesAsync(0);

        var db = fixture.CreateContext();
        var logger = new Mock<ILogger<SimulateMatchesJob>>();
        var job = new SimulateMatchesJob(db, logger.Object);

        // Act
        await job.SimulateMatchPassesAsync();

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) => o.ToString()!.Contains("Worker done. Processed 0 matches")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenMoreMatchesThanBatchSize_WhenSimulated_ThenProcessesInMultipleBatches()
    {
        // Arrange
        await fixture.SeedMatchesAsync(1200);

        var db = fixture.CreateContext();
        var logger = new Mock<ILogger<SimulateMatchesJob>>();
        var job = new SimulateMatchesJob(db, logger.Object);

        // Act
        await job.SimulateMatchPassesAsync();

        // Assert
        db.ChangeTracker.Clear();
        var processed = await db.Matches.CountAsync(m => m.TotalPasses != null);
        processed.Should().Be(1200);

        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) => o.ToString()!.Contains("Batch complete")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeast(3));
    }

    [Fact]
    public async Task GivenConcurrentWorkers_WhenSimulated_ThenNoMatchProcessedTwice()
    {
        // Arrange
        await fixture.SeedMatchesAsync(1000);

        var tasks = Enumerable.Range(0, 3).Select(_ =>
        {
            var db = fixture.CreateContext();
            var logger = new Mock<ILogger<SimulateMatchesJob>>();
            var job = new SimulateMatchesJob(db, logger.Object);
            return job.SimulateMatchPassesAsync();
        });

        // Act
        await Task.WhenAll(tasks);

        // Assert
        await using var verifyDb = fixture.CreateContext();
        var matches = await verifyDb.Matches.ToListAsync();

        matches.Should().HaveCount(1000);
        matches.Should().AllSatisfy(m => m.TotalPasses.Should().NotBeNull());
    }
}
