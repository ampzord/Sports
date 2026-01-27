using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Entities;

namespace Sports.MatchSimulator.Features.MatchSimulation;

public class MatchSimulationJob
{
    private const int MinTotalPasses = 100;
    private const int MaxTotalPasses = 1001;
    private const int BatchSize = 5000;

    private readonly SportsDbContext _db;
    private readonly ILogger<MatchSimulationJob> _logger;

    public MatchSimulationJob(
        SportsDbContext db,
        ILogger<MatchSimulationJob> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SimulateMatchPassesAsync()
    {
        _logger.LogInformation("Starting match simulation");

        try
        {
            int processed = 0;
            Random random = new();

            while (true)
            {
                List<Match> batch = await _db.Matches
                    .Where(m => m.TotalPasses == null)
                    .Take(BatchSize)
                    .ToListAsync();

                if (batch.Count == 0)
                {
                    break;
                }

                foreach (Match match in batch)
                {
                    match.TotalPasses = random.Next(
                        MinTotalPasses,
                        MaxTotalPasses);
                }

                _ = await _db.SaveChangesAsync();
                processed += batch.Count;

                _logger.LogInformation(
                    "Processed {Count} matches",
                    processed);

                _db.ChangeTracker.Clear();
            }

            _logger.LogInformation(
                "Completed simulation for {Count} matches",
                processed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during match simulation");
            throw;
        }
    }
}