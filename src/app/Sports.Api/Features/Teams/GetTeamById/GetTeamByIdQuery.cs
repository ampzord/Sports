using Sports.Api.Features.Teams._Shared;

namespace Sports.Api.Features.Teams.GetTeamById;


using ErrorOr;
using MediatR;

public record GetTeamByIdQuery(int Id) : IRequest<ErrorOr<TeamResponse>>;

