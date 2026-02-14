namespace Sports.Api.Features.Matches.AddMatch;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Matches._Shared;

public record AddMatchCommand(
    int LeagueId,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;
