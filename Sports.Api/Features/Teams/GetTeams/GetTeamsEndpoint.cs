namespace Sports.Api.Features.Teams.GetTeams;

using Sports.Api.Features.Teams._Shared.Responses;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetTeamsEndpoint(IMediator mediator) :
    Endpoint<GetTeamsRequest, List<TeamResponse>>
{

    public override void Configure()
    {
        Get("/api/teams");
        AllowAnonymous();
        Description(b => b
            .Produces<List<TeamResponse>>(200)
            .WithTags("Teams"));
        Summary(s =>
        {
            s.Summary = "Get all teams";
        });
    }

    public override async Task HandleAsync(
        GetTeamsRequest req,
        CancellationToken ct)
    {
        var query = new GetTeamsQuery(req.LeagueId);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
