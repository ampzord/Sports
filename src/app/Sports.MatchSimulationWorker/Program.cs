using Serilog;
using Sports.MatchSimulationWorker.DependencyInjection;
using Sports.MatchSimulationWorker.Jobs;
using Sports.MatchSimulationWorker.Services;
using Sports.Shared.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSerilog(configuration =>
{
    configuration.ConfigureSerilog(builder.Configuration, "sports-worker");
}, writeToProviders: true);

builder.AddRabbitMQClient("messaging");

builder.Services
    .RegisterDatabase(builder.Configuration)
    .AddTransient<ISimulateMatchesJob, SimulateMatchesJob>();

builder.Services.AddHostedService<MatchSimulationConsumer>();

var host = builder.Build();
host.Run();
