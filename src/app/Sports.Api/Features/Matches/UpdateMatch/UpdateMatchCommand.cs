namespace Sports.Api.Features.Matches.UpdateMatch;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Matches._Shared;

public record UpdateMatchCommand(
    Guid Id,
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? TotalPasses
) : IRequest<ErrorOr<MatchResponse>>;
