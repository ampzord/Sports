namespace Sports.Api.Features.Leagues.AddLeague;

using MediatR;

public record AddLeagueCommand(
    string Name
) : IRequest<AddLeagueResponse>;