namespace Sports.Api.Features.Matches.UpdateMatch;

using MediatR;

public record UpdateMatchCommand(
    int Id,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<UpdateMatchResponse?>;