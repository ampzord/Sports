using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Sports.MatchSimulationWorker.Database;
using Sports.MatchSimulationWorker.Jobs;
using Sports.MatchSimulationWorker.Services;
using Sports.Shared.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var lokiUrl = builder.Configuration["Loki:Endpoint"] ?? "http://localhost:3100";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(lokiUrl, new[]
    {
        new LokiLabel { Key = "app", Value = "sports-worker" }
    })
    .CreateLogger();

builder.Services.AddSerilog();

builder.AddRabbitMQClient("messaging");

var connectionString = builder.Configuration.GetConnectionString(ConnectionStrings.SportsDB)
?? throw new InvalidOperationException($"Connection string '{ConnectionStrings.SportsDB}' not found.");

builder.Services.AddDbContext<MatchSimulationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<ISimulateMatchesJob, SimulateMatchesJobDeadlock>();

builder.Services.AddHostedService<MatchSimulationConsumer>();

var host = builder.Build();
host.Run();
