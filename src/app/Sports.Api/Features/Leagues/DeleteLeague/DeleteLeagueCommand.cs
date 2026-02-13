namespace Sports.Api.Features.Leagues.DeleteLeague;

using ErrorOr;
using MediatR;

public record DeleteLeagueCommand(int Id) : IRequest<ErrorOr<Deleted>>;