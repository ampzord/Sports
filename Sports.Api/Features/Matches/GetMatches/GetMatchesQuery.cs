namespace Sports.Api.Features.Matches.GetMatches;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetMatchesQuery(int? LeagueId) : IRequest<ErrorOr<List<MatchResponse>>>;
