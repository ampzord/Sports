namespace Sports.Api.Features.Leagues.DeleteLeague;

using ErrorOr;
using MediatR;

public record DeleteLeagueCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;