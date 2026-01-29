namespace Sports.Api.DependencyInjection;

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

public static class MappersExtensions
{
    public static IServiceCollection RegisterMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<AddPlayerMapper>();
        services.AddSingleton<UpdatePlayerMapper>();
        services.AddSingleton<GetPlayerMapper>();
        services.AddSingleton<GetPlayersMapper>();

        services.AddSingleton<AddTeamMapper>();
        services.AddSingleton<UpdateTeamMapper>();
        services.AddSingleton<GetTeamMapper>();
        services.AddSingleton<DeleteTeamMapper>();

        services.AddSingleton<AddLeagueMapper>();
        services.AddSingleton<UpdateLeagueMapper>();
        services.AddSingleton<GetLeagueMapper>();
        services.AddSingleton<DeleteLeagueMapper>();

        services.AddSingleton<AddMatchMapper>();
        services.AddSingleton<UpdateMatchMapper>();
        services.AddSingleton<GetMatchMapper>();
        services.AddSingleton<DeleteMatchMapper>();

        return services;
    }
}