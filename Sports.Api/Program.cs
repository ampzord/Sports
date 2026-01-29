using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sports.Api.Behaviors;
using Sports.Api.Database;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.GetLeague;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.GetMatch;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.GetPlayer;
using Sports.Api.Features.Players.GetPlayers;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeam;
using Sports.Api.Features.Teams.UpdateTeam;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

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
        typeof(ExceptionHandlingBehavior<,>));
    config.AddOpenBehavior(
        typeof(LoggingBehavior<,>));
});

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter(
            allowIntegerValues: false)));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddSingleton<AddPlayerMapper>();
builder.Services.AddSingleton<UpdatePlayerMapper>();
builder.Services.AddSingleton<GetPlayerMapper>();
builder.Services.AddSingleton<GetPlayersMapper>();

builder.Services.AddSingleton<AddTeamMapper>();
builder.Services.AddSingleton<UpdateTeamMapper>();
builder.Services.AddSingleton<GetTeamMapper>();
builder.Services.AddSingleton<DeleteTeamMapper>();

builder.Services.AddSingleton<AddLeagueMapper>();
builder.Services.AddSingleton<UpdateLeagueMapper>();
builder.Services.AddSingleton<GetLeagueMapper>();
builder.Services.AddSingleton<DeleteLeagueMapper>();

builder.Services.AddSingleton<AddMatchMapper>();
builder.Services.AddSingleton<UpdateMatchMapper>();
builder.Services.AddSingleton<GetMatchMapper>();
builder.Services.AddSingleton<DeleteMatchMapper>();

builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o => o.DocumentSettings = s =>
{
    s.Title = "Sports API";
    s.Version = "v1";
    s.Description = "API for managing players, teams, leagues and matches";
});

builder.Services.AddCors(options =>
    options.AddPolicy("AllowLocalhost", policy =>
        policy.WithOrigins(
            "http://localhost:5173",
            "https://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

app.UseFastEndpoints(
   c => c.Errors.UseProblemDetails(
       x =>
       {
           x.AllowDuplicateErrors = true;  //allows duplicate errors for the same error name
           x.IndicateErrorCode = true;     //serializes the fluentvalidation error code
           x.IndicateErrorSeverity = true; //serializes the fluentvalidation error severity
           x.TypeValue = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
           x.TitleValue = "One or more validation errors occurred.";
           x.TitleTransformer = pd => pd.Status switch
           {
               400 => "Validation Error",
               404 => "Not Found",
               _ => "One or more errors occurred!"
           };
       }));

app.Run();