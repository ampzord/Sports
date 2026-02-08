namespace Sports.Api.Features.Matches.AddMatch;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;

public record AddMatchCommand(
    int LeagueId,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;