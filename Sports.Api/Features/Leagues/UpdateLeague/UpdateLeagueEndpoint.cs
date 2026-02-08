

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Leagues._Shared.Responses;

public class UpdateLeagueEndpoint(IMediator mediator, LeagueMapper mapper) : Endpoint<UpdateLeagueRequest, LeagueResponse>
{

    public override void Configure()
    {
        Put("/api/leagues/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<LeagueResponse>(200)
            .Produces(400)
            .Produces(404)
            .Produces(409)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Update an existing league";
            s.ExampleRequest = UpdateLeagueRequest.Example;
        });
    }

    public override async Task HandleAsync(
        UpdateLeagueRequest req,
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