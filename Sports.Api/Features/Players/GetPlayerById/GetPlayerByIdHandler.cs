namespace Sports.Api.Features.Players.GetPlayerById;

using Sports.Api.Features.Players._Shared;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class GetPlayerByIdHandler(SportsDbContext db, PlayerMapper mapper)
    : IRequestHandler<GetPlayerByIdQuery, ErrorOr<PlayerResponse>>
{
    public async Task<ErrorOr<PlayerResponse>> Handle(
        GetPlayerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var player = await db.Players.FindAsync([query.Id], cancellationToken);

        if (player is null)
            return Error.NotFound("Player.NotFound", "Player not found");

        return mapper.ToResponse(player);
    }
}