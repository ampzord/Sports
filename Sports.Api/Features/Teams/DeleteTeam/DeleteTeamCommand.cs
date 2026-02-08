namespace Sports.Api.Features.Teams.DeleteTeam;

using ErrorOr;
using MediatR;

public record DeleteTeamCommand(int Id) : IRequest<ErrorOr<Deleted>>;