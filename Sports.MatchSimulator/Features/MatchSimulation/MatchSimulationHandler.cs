
using Hangfire;
using MediatR;

namespace Sports.MatchSimulator.Features.MatchSimulation;

public class MatchSimulationHandler
    : IRequestHandler<MatchSimulationCommand, MatchSimulationResponse>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<MatchSimulationHandler> _logger;

    public MatchSimulationHandler(
        IBackgroundJobClient backgroundJobClient,
        ILogger<MatchSimulationHandler> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public Task<MatchSimulationResponse> Handle(
        MatchSimulationCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            string jobId = _backgroundJobClient.Enqueue<MatchSimulationJob>(
                job => job.SimulateMatchPassesAsync());

            _logger.LogInformation(
                "Enqueued match simulation job: {JobId}",
                jobId);

            return Task.FromResult(new MatchSimulationResponse
            {
                JobId = jobId,
                Message = "Match simulation started",
                Status = "Queued"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enqueue job");
            throw;
        }
    }
}