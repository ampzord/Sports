namespace Sports.Api.Features.Matches.GetMatches;

using FastEndpoints;

public record GetMatchesRequest([property: QueryParam] int? LeagueId);
