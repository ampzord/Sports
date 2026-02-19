namespace Sports.Api.Features.Players.GetPlayerById;

using Sports.Api.Features.Players._Shared;
using ErrorOr;
using MediatR;
using Sports.Api.Database;
using Sports.Domain.PlayerAggregate.ValueObjects;

public class GetPlayerByIdHandler(SportsDbContext db, PlayerMapper mapper)
    : IRequestHandler<GetPlayerByIdQuery, ErrorOr<PlayerResponse>>
{
    public async Task<ErrorOr<PlayerResponse>> Handle(
        GetPlayerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var player = await db.Players.FindAsync([PlayerId.Create(query.Id)], cancellationToken);

        if (player is null)
            return PlayerErrors.NotFound;

        return mapper.ToResponse(player);
    }
}
