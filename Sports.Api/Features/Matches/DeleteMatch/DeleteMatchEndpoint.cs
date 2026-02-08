namespace Sports.Api.Features.Matches.DeleteMatch;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Matches._Shared;

public class DeleteMatchEndpoint(IMediator mediator, MatchMapper mapper) : Endpoint<DeleteMatchRequest>
{
    public override void Configure()
    {
        Delete("/api/matches/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .Produces(404)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Delete a match";
            s.ExampleRequest = DeleteMatchRequest.Example;
        });
    }

    public override async Task HandleAsync(
        DeleteMatchRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}