namespace Sports.Api.Features.Teams.GetTeams;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Teams._Shared;
using System.Collections.Immutable;

public class GetTeamsEndpoint(IMediator mediator) :
    Endpoint<GetTeamsRequest, ImmutableList<TeamResponse>>
{

    public override void Configure()
    {
        Get("teams");
        AllowAnonymous();
        Description(b => b
            .Produces<ImmutableList<TeamResponse>>(200)
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
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
