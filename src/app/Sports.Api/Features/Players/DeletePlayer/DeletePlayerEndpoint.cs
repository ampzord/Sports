namespace Sports.Api.Features.Players.DeletePlayer;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class DeletePlayerEndpoint(IMediator mediator) : Endpoint<DeletePlayerRequest>
{

    public override void Configure()
    {
        Delete("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .Produces(404)
            .WithTags("Players"));
        Summary(s =>
        {
            s.Summary = "Delete a player";
        });
    }

    public override async Task HandleAsync(
        DeletePlayerRequest req,
        CancellationToken ct)
    {
        var command = new DeletePlayerCommand(req.Id);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}
