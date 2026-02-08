

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

namespace Sports.Api.Features.Teams.UpdateTeam;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;

public class UpdateTeamEndpoint(IMediator mediator, TeamMapper mapper) : Endpoint<UpdateTeamRequest, TeamResponse>
{

    public override void Configure()
    {
        Put("/api/teams/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<TeamResponse>(200)
            .Produces(400)
            .Produces(404)
            .Produces(409)
            .WithTags("Teams"));
        Summary(s =>
        {
            s.Summary = "Update an existing team";
            s.ExampleRequest = UpdateTeamRequest.Example;
        });
    }

    public override async Task HandleAsync(
        UpdateTeamRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}