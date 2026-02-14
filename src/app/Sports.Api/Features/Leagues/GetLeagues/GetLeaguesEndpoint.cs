namespace Sports.Api.Features.Leagues.GetLeagues;

using FastEndpoints;
using MediatR;
using Sports.Api.Features.Leagues._Shared;
using System.Collections.Immutable;

public class GetLeaguesEndpoint(IMediator mediator) : EndpointWithoutRequest<ImmutableList<LeagueResponse>>
{

    public override void Configure()
    {
        Get("leagues");
        AllowAnonymous();
        Description(b => b
            .Produces<ImmutableList<LeagueResponse>>(200)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Get all leagues";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetLeaguesQuery();
        var response = await mediator.Send(query, ct);
        await Send.OkAsync(response, ct);
    }
}
