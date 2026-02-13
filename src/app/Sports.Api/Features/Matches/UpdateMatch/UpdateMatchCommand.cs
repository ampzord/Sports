using Sports.Api.Features.Matches._Shared;

namespace Sports.Api.Features.Matches.UpdateMatch;


using ErrorOr;
using MediatR;

public record UpdateMatchCommand(
    int Id,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;