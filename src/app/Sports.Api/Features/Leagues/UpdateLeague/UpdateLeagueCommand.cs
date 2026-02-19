namespace Sports.Api.Features.Leagues.UpdateLeague;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Leagues._Shared;

public record UpdateLeagueCommand(
    Guid Id,
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;
