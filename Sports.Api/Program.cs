using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Behaviours;
using Sports.Api.Database;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.DeletePlayer;
using Sports.Api.Features.Players.GetPlayer;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeam;
using Sports.Api.Features.Teams.UpdateTeam;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SportsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(
        typeof(Program).Assembly);

    config.AddOpenBehavior(
        typeof(LoggingBehaviour<,>));
});

builder.Services.AddSingleton<AddPlayerMapper>();
builder.Services.AddSingleton<UpdatePlayerMapper>();
builder.Services.AddSingleton<GetPlayerMapper>();
builder.Services.AddSingleton<DeletePlayerMapper>();

builder.Services.AddSingleton<AddTeamMapper>();
builder.Services.AddSingleton<UpdateTeamMapper>();
builder.Services.AddSingleton<GetTeamMapper>();
builder.Services.AddSingleton<DeleteTeamMapper>();

builder.Services.AddSingleton<AddLeagueMapper>();

builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o => o.DocumentSettings = s =>
    {
        s.Title = "Sports API";
        s.Version = "v1";
        s.Description = "API for managing players, teams, leagues and matches";
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();
