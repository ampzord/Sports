namespace Sports.Api.Features.Teams.GetTeamById;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class GetTeamByIdHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<GetTeamByIdQuery, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> Handle(
        GetTeamByIdQuery query,
        CancellationToken cancellationToken)
    {
        var team = await db.Teams.FindAsync([query.Id], cancellationToken);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found");

        return mapper.ToResponse(team);
    }
}