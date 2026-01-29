using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;
using Sports.Api.DependencyInjection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services
    .RegisterDatabase(builder.Configuration)
    .RegisterMediatR()
    .RegisterMappers()
    .RegisterApi()
    .RegisterCors();

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
            x.AllowDuplicateErrors = true;
            x.IndicateErrorCode = true;
            x.IndicateErrorSeverity = true;
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