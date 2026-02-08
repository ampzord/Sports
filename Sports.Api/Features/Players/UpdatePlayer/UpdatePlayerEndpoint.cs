

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

namespace Sports.Api.Features.Players.UpdatePlayer;

using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Players._Shared.Responses;

public class UpdatePlayerEndpoint(IMediator mediator, PlayerMapper mapper) : Endpoint<UpdatePlayerRequest, PlayerResponse>
{

    public override void Configure()
    {
        Put("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<PlayerResponse>(200)
            .Produces(400)
            .Produces(404)
            .Produces(409)
            .WithTags("Players"));
        Summary(s =>
        {
            s.Summary = "Update an existing player";
            s.ExampleRequest = UpdatePlayerRequest.Example;
        });
    }

    public override async Task HandleAsync(
        UpdatePlayerRequest req,
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