namespace Sports.Api.Features.Teams.DeleteTeam;

using ErrorOr;
using MediatR;

public record DeleteTeamCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;