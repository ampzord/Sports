namespace Sports.Api.Features.Teams.GetTeams;

using FastEndpoints;

public record GetTeamsRequest([property: QueryParam] int? LeagueId);
