using FastEndpoints;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.MatchSimulator.Features.MatchSimulation;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<SportsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly()));

builder.Services.AddFastEndpoints();

builder.Services.AddScoped<MatchSimulationJob>();

builder.Services.AddHangfire(config =>
    config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

WebApplication app = builder.Build();

app.UseHangfireDashboard("/hangfire");

app.UseFastEndpoints();

app.Run();