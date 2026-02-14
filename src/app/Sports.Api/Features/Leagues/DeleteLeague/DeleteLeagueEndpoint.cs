namespace Sports.Api.Features.Leagues.DeleteLeague;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Leagues._Shared;

public class DeleteLeagueEndpoint(IMediator mediator, LeagueMapper mapper) : Endpoint<DeleteLeagueRequest>
{

    public override void Configure()
    {
        Delete("leagues/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .Produces(404)
            .Produces(409)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Delete a league";
            s.ExampleRequest = DeleteLeagueRequest.Example;
        });
    }

    public override async Task HandleAsync(
        DeleteLeagueRequest req,
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
