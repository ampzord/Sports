namespace Sports.Api.Features.Teams.GetTeamById;

using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetTeamByIdQuery(int Id) : IRequest<ErrorOr<TeamResponse>>;

