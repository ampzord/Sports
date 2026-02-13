using Sports.Api.Features.Matches._Shared;

namespace Sports.Api.Features.Matches.AddMatch;


using ErrorOr;
using MediatR;

public record AddMatchCommand(
    int LeagueId,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;