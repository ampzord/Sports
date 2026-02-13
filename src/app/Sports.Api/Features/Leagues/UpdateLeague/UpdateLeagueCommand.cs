using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.UpdateLeague;


using ErrorOr;
using MediatR;

public record UpdateLeagueCommand(
    int Id,
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;