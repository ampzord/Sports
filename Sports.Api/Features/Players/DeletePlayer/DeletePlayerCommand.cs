namespace Sports.Api.Features.Players.DeletePlayer;

using ErrorOr;
using MediatR;

public record DeletePlayerCommand(int Id) : IRequest<ErrorOr<Deleted>>;