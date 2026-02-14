namespace Sports.Api.Features.Leagues.GetLeagueById;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Leagues._Shared;

public record GetLeagueByIdQuery(int Id) : IRequest<ErrorOr<LeagueResponse>>;
