namespace Sports.Api.Features.Matches.AddMatch;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Matches._Shared;

public record AddMatchCommand(
    Guid LeagueId,
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;
