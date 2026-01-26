namespace Sports.Api.Features.Leagues.UpdateLeague;

using MediatR;

public record UpdateLeagueCommand(
    int Id,
    string Name
) : IRequest<UpdateLeagueResponse?>;