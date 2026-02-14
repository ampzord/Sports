namespace Sports.Api.Features.Matches.GetMatches;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Matches._Shared;
using System.Collections.Immutable;

public record GetMatchesQuery(int? LeagueId) : IRequest<ErrorOr<ImmutableList<MatchResponse>>>;
