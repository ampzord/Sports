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
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
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
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var job = scope.ServiceProvider.GetRequiredService<ISimulateMatchesJob>();
                    await job.SimulateMatchPassesAsync();

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during match simulation");
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            }, cancellationToken);
        };

        channel.BasicConsume(
            queue: QueueNames.MatchSimulation,
            autoAck: false,
            consumer: consumer);

        return Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
