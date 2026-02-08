namespace Sports.MatchSimulationWorker.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sports.MatchSimulationWorker.Jobs;
using Sports.Shared.Messaging;

public class MatchSimulationConsumer(
    IConnection rabbitConnection,
    IServiceScopeFactory scopeFactory,
    ILogger<MatchSimulationConsumer> logger) : BackgroundService
{
    private int _activeJobs;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Match simulation consumer started, waiting for messages");

        var channel = rabbitConnection.CreateModel();

        channel.QueueDeclare(
            QueueNames.MatchSimulation,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (_, ea) =>
        {
            _ = Task.Run(async () =>
            {
                var current = Interlocked.Increment(ref _activeJobs);
                logger.LogInformation("Simulation started — active jobs: {ActiveJobs}", current);

                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var job = scope.ServiceProvider.GetRequiredService<ISimulateMatchesJob>();
                    await job.SimulateMatchPassesAsync();

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                    logger.LogInformation("Simulation completed — active jobs: {ActiveJobs}",
                        Interlocked.Decrement(ref _activeJobs));
                }
                catch (Exception ex)
                {
                    Interlocked.Decrement(ref _activeJobs);
                    logger.LogError(ex, "Error during match simulation");
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            }, stoppingToken);
        };

        channel.BasicConsume(
            queue: QueueNames.MatchSimulation,
            autoAck: false,
            consumer: consumer);

        return Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
