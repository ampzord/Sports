using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Entities;

namespace Sports.MatchSimulator.Features.MatchSimulation;

public class MatchSimulationJob
{
    private const int MinTotalPasses = 100;
    private const int MaxTotalPasses = 1001;

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
            List<Match> matchesWithoutPasses = await _db.Matches
                .Where(m => m.TotalPasses == null)
                .ToListAsync();

            if (matchesWithoutPasses.Count == 0)
            {
                _logger.LogInformation("No matches to simulate");
                return;
            }

            Random random = new();

            foreach (Match? match in matchesWithoutPasses)
            {
                match.TotalPasses = random.Next(MinTotalPasses, MaxTotalPasses);
                _logger.LogInformation(
                    "Simulated match {MatchId} with {Passes} passes",
                    match.Id,
                    match.TotalPasses);
            }

            _ = await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Completed simulation for {Count} matches",
                matchesWithoutPasses.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during match simulation");
            throw;
        }
    }
}