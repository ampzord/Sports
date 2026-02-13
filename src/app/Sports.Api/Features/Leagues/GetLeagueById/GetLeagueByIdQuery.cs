using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.GetLeagueById;


using ErrorOr;
using MediatR;

public record GetLeagueByIdQuery(int Id) : IRequest<ErrorOr<LeagueResponse>>;