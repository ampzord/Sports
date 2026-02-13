using FastEndpoints;

namespace Sports.Api.Features.Matches.GetMatches;

public record GetMatchesRequest([property: QueryParam] int? LeagueId);
