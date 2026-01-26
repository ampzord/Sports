namespace Sports.Api.Features.Matches.AddMatch;

using MediatR;

public record AddMatchCommand(
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses
) : IRequest<AddMatchResponse>;