namespace Sports.Api.DependencyInjection;

using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;

public static class ApiExtensions
{
    public static IServiceCollection RegisterApi(
        this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter(
                    allowIntegerValues: false)));

        services.AddFluentValidationAutoValidation();
        services.AddOpenApi();
        services.AddFastEndpoints();

        services.SwaggerDocument(o => o.DocumentSettings = s =>
        {
            s.Title = "Sports API";
            s.Version = "v1";
            s.Description = "API for managing players, teams, "
                + "leagues and matches";
        });

        return services;
    }
}