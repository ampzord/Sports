using Sports.Api.Features.Matches._Shared;

namespace Sports.Api.Features.Matches.GetMatches;


using ErrorOr;
using MediatR;

public record GetMatchesQuery(int? LeagueId) : IRequest<ErrorOr<List<MatchResponse>>>;
