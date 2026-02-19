namespace Sports.Api.Features.Teams.GetTeamById;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Teams._Shared;

public record GetTeamByIdQuery(Guid Id) : IRequest<ErrorOr<TeamResponse>>;

