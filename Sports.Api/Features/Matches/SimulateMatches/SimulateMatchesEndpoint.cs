namespace Sports.Api.Features.Matches.SimulateMatches;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class SimulateMatchesEndpoint(IMediator mediator)
    : EndpointWithoutRequest<SimulateMatchesResponse>
{
    public override void Configure()
    {
        Post("/api/matches/simulate");
        Description(x => x
            .Produces<SimulateMatchesResponse>(200)
            .WithTags("Matches"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var command = new SimulateMatchesCommand();
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await Send.OkAsync(result.Value, cancellation: ct);
    }
}
