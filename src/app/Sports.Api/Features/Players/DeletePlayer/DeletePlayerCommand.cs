namespace Sports.Api.Features.Players.DeletePlayer;

using ErrorOr;
using MediatR;

public record DeletePlayerCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;