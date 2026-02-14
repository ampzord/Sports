namespace Sports.Api.Features.Teams.GetTeamById;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Teams._Shared;

public class GetTeamByIdEndpoint(IMediator mediator) : Endpoint<GetTeamByIdRequest, TeamResponse>
{

    public override void Configure()
    {
        Get("teams/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<TeamResponse>(200)
            .Produces(404)
            .WithTags("Teams"));
        Summary(s =>
        {
            s.Summary = "Get a team by ID";
        });
    }

    public override async Task HandleAsync(
        GetTeamByIdRequest req,
        CancellationToken ct)
    {
        var query = new GetTeamByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
