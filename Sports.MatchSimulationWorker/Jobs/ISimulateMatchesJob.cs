namespace Sports.MatchSimulationWorker.Jobs;

public interface ISimulateMatchesJob
{
    const int MinPasses = 100;
    const int MaxPasses = 1001;

    Task SimulateMatchPassesAsync();
}
