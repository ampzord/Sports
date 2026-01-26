namespace Sports.Api.Features.Leagues.DeleteLeague;

using MediatR;

public record DeleteLeagueCommand(int Id) : IRequest<DeleteLeagueResponse>;