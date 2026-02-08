namespace Sports.Api.Features.Matches.SimulateMatches;

using System.Text;
using ErrorOr;
using MediatR;
using RabbitMQ.Client;
using Sports.Shared.Messaging;

public class SimulateMatchesHandler(
    IConnection rabbitConnection,
    ILogger<SimulateMatchesHandler> logger)
    : IRequestHandler<SimulateMatchesCommand, ErrorOr<SimulateMatchesResponse>>
{
    public Task<ErrorOr<SimulateMatchesResponse>> Handle(
        SimulateMatchesCommand command,
        CancellationToken cancellationToken)
    {
        using var channel = rabbitConnection.CreateModel();

        channel.QueueDeclare(
            QueueNames.MatchSimulation,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes("simulate");
        channel.BasicPublish(
            exchange: "",
            routingKey: QueueNames.MatchSimulation,
            basicProperties: null,
            body: body);

        logger.LogInformation("Published match simulation message to queue");

        ErrorOr<SimulateMatchesResponse> response = new SimulateMatchesResponse
        {
            Message = "Simulation started for all matches",
            Status = "Queued"
        };

        return Task.FromResult(response);
    }
}
