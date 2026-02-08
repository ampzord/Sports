using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Sports.Api.Database;
using Sports.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var lokiUrl = builder.Configuration["Loki:Endpoint"] ?? "http://localhost:3100";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(lokiUrl, new[]
    {
        new LokiLabel { Key = "app", Value = "sports-api" }
    })
    .CreateLogger();

builder.Host.UseSerilog();

builder.AddRabbitMQClient("messaging");

builder.Services
.AddProblemDetails()
.RegisterDatabase(builder.Configuration)
.RegisterMediatR()
.RegisterMappers()
.RegisterApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SportsDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerGen(
        uiConfig: ui =>
        {
            ui.OperationsSorter = "method";
            ui.TagsSorter = "alpha";
        });
}

app.UseHttpsRedirection();

app.UseStatusCodePages();

app
    .UseFastEndpoints(c => c.Errors.UseProblemDetails(x =>
    {
        x.AllowDuplicateErrors = true;
        x.IndicateErrorCode = true;
        x.IndicateErrorSeverity = true;
        x.TypeValue =
            "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
        x.TitleValue =
            "One or more validation errors occurred.";
        x.TitleTransformer = pd => pd.Status switch
        {
            400 => "Validation Error",
            404 => "Not Found",
            _ => "One or more errors occurred!"
        };
    }));

app.MapDefaultEndpoints();

app.Run();