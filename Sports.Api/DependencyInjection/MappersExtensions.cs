namespace Sports.Api.DependencyInjection;

using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Teams._Shared;

public static class MappersExtensions
{
    public static IServiceCollection RegisterMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<LeagueMapper>();
        services.AddSingleton<TeamMapper>();
        services.AddSingleton<PlayerMapper>();
        services.AddSingleton<MatchMapper>();

        return services;
    }
}