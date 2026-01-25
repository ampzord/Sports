namespace Sports.Api.Features.Players.AddPlayer;

using MediatR;

public record AddPlayerCommand(
    string Name,
    string Position,
    int? TeamId
) : IRequest<AddPlayerResponse>;
