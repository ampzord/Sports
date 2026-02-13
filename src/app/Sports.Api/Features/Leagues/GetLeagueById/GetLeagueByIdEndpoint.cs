using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.GetLeagueById;


using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetLeagueByIdEndpoint(IMediator mediator) : Endpoint<GetLeagueByIdRequest, LeagueResponse>
{

    public override void Configure()
    {
        Get("/api/leagues/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<LeagueResponse>(200)
            .Produces(404)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Get a league by ID";
        });
    }

    public override async Task HandleAsync(
        GetLeagueByIdRequest req,
        CancellationToken ct)
    {
        var query = new GetLeagueByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
