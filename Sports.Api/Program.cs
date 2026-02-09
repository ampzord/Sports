using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sports.Api.Database;
using Sports.Api.DependencyInjection;
using Sports.Api.Extensions;
using Sports.Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ConfigureSerilog(context.Configuration, "sports-api");
}, writeToProviders: true);

builder.AddRabbitMQClient("messaging");

builder.Services
.AddProblemDetails()
.AddExceptionHandler<GlobalExceptionHandler>()
.RegisterDatabase(builder.Configuration)
.RegisterMediatR()
.RegisterMappers()
.RegisterApi();

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<SportsDbContext>();
    await db.Database.MigrateAsync();
}
catch (Exception ex)
{
    Log.Warning(ex, "Database migration failed — the database may be unavailable");
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
        "https://tools.ietf.org/html/rfc9110#section-15.5.1";
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