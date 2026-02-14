namespace Sports.Api.Features.Teams.DeleteTeam;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Teams._Shared;

public class DeleteTeamEndpoint(IMediator mediator, TeamMapper mapper) : Endpoint<DeleteTeamRequest>
{

    public override void Configure()
    {
        Delete("teams/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .Produces(404)
            .Produces(409)
            .WithTags("Teams"));
        Summary(s =>
        {
            s.Summary = "Delete a team";
            s.ExampleRequest = DeleteTeamRequest.Example;
        });
    }

    public override async Task HandleAsync(
        DeleteTeamRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}
