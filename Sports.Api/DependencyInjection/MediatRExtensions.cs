namespace Sports.Api.DependencyInjection;

using Sports.Shared.Behaviors;

public static class MediatRExtensions
{
    public static IServiceCollection RegisterMediatR(
        this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                typeof(Program).Assembly);

            config.AddOpenBehavior(
                typeof(LoggingBehavior<,>));
        });

        return services;
    }
}