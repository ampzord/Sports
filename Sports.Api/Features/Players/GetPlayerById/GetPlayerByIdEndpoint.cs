namespace Sports.Api.Features.Players.GetPlayerById;

using Sports.Api.Features.Players._Shared.Responses;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetPlayerByIdEndpoint(IMediator mediator) : Endpoint<GetPlayerByIdRequest, PlayerResponse>
{

    public override void Configure()
    {
        Get("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<PlayerResponse>(200)
            .Produces(404)
            .WithTags("Players"));
        Summary(s =>
        {
            s.Summary = "Get a player by ID";
        });
    }

    public override async Task HandleAsync(
        GetPlayerByIdRequest req,
        CancellationToken ct)
    {
        var query = new GetPlayerByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
