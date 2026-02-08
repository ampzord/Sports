namespace Sports.Api.DependencyInjection;

using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.GetLeagueById;
using Sports.Api.Features.Leagues.GetLeagues;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.GetMatchById;
using Sports.Api.Features.Matches.GetMatches;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.GetPlayerById;
using Sports.Api.Features.Players.GetPlayers;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.GetTeamById;
using Sports.Api.Features.Teams.GetTeams;

public static class MappersExtensions
{
    public static IServiceCollection RegisterMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<AddPlayerMapper>();
        services.AddSingleton<UpdatePlayerMapper>();
        services.AddSingleton<GetPlayerByIdMapper>();
        services.AddSingleton<GetPlayersMapper>();

        services.AddSingleton<AddTeamMapper>();
        services.AddSingleton<UpdateTeamMapper>();
        services.AddSingleton<GetTeamsMapper>();
        services.AddSingleton<GetTeamByIdMapper>();
        services.AddSingleton<DeleteTeamMapper>();

        services.AddSingleton<AddLeagueMapper>();
        services.AddSingleton<UpdateLeagueMapper>();
        services.AddSingleton<GetLeaguesMapper>();
        services.AddSingleton<GetLeagueByIdMapper>();
        services.AddSingleton<DeleteLeagueMapper>();

        services.AddSingleton<AddMatchMapper>();
        services.AddSingleton<UpdateMatchMapper>();
        services.AddSingleton<GetMatchByIdMapper>();
        services.AddSingleton<GetMatchesMapper>();
        services.AddSingleton<DeleteMatchMapper>();

        return services;
    }
}