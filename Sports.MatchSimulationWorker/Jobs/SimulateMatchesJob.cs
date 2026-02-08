namespace Sports.MatchSimulationWorker.Jobs;

using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sports.MatchSimulationWorker.Database;

public class SimulateMatchesJob(
    MatchSimulationDbContext db,
    ILogger<SimulateMatchesJob> logger) : ISimulateMatchesJob
{
    private const int BatchSize = 500;

    private const string BatchQuery =
        """
        SELECT TOP ({0}) *
        FROM Matches WITH (UPDLOCK, ROWLOCK, READPAST)
        WHERE TotalPasses IS NULL
        ORDER BY Id
        """;

    public async Task SimulateMatchPassesAsync()
    {
        var totalProcessed = 0;
        var random = new Random();

        while (true)
        {
            var batchCount = await ProcessBatchAsync(random);

            if (batchCount == 0)
            {
                logger.LogInformation(
                    "Worker done. Processed {Total} matches",
                    totalProcessed
                );
                return;
            }

            totalProcessed += batchCount;

            logger.LogInformation(
                "Batch complete: {Count} matches. Total: {Total}",
                batchCount,
                totalProcessed
            );
        }
    }

    private async Task<int> ProcessBatchAsync(Random random)
    {
        await using var transaction = await db.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);

        var matches = await db.Matches
            .FromSqlRaw(BatchQuery, BatchSize)
            .ToListAsync();

        if (matches.Count == 0)
            return 0;

        foreach (var match in matches)
        {
            match.TotalPasses = random.Next(
                ISimulateMatchesJob.MinPasses,
                ISimulateMatchesJob.MaxPasses
            );
        }

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return matches.Count;
    }
}