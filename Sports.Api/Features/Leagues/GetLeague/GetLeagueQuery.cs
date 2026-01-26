namespace Sports.Api.Features.Leagues.GetLeague;

using MediatR;

public record GetLeagueQuery(int Id) : IRequest<GetLeagueResponse?>;