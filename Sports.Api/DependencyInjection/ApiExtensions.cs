namespace Sports.Api.DependencyInjection;

using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class ApiExtensions
{
    public static IServiceCollection RegisterApi(
        this IServiceCollection services)
    {
        services.AddOpenApi();

        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(allowIntegerValues: false));
        });

        services.AddFastEndpoints();

        services.SwaggerDocument(o =>
        {
            o.SerializerSettings = s =>
            {
                s.Converters.Add(
                    new JsonStringEnumConverter(
                        allowIntegerValues: false));
            };

            o.DocumentSettings = s =>
            {
                s.Title = "Sports API";
                s.Version = "v1";
                s.Description =
                    "API for managing players, teams, "
                    + "leagues and matches";
            };
        });

        return services;
    }
}