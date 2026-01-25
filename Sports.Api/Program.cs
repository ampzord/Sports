using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.DeletePlayer;
using Sports.Api.Features.Players.GetPlayer;
using Sports.Api.Features.Players.UpdatePlayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SportsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

builder.Services.AddSingleton<AddPlayerMapper>();
builder.Services.AddSingleton<UpdatePlayerMapper>();
builder.Services.AddSingleton<GetPlayerMapper>();
builder.Services.AddSingleton<DeletePlayerMapper>();

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
