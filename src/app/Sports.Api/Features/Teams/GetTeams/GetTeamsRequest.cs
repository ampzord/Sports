using FastEndpoints;

namespace Sports.Api.Features.Teams.GetTeams;

public record GetTeamsRequest([property: QueryParam] int? LeagueId);
