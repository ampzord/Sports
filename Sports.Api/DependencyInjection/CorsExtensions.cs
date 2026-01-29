namespace Sports.Api.DependencyInjection;

public static class CorsExtensions
{
    public static IServiceCollection RegisterCors(
        this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("AllowLocalhost", policy =>
                policy.WithOrigins(
                    "http://localhost:5173",
                    "https://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

        return services;
    }
}