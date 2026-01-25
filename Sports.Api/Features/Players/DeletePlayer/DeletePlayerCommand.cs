namespace Sports.Api.Features.Players.DeletePlayer;

using MediatR;

public record DeletePlayerCommand(int Id) : IRequest<bool>;