namespace Sports.Api.Features.Teams.AddTeam;

using Sports.Api.Features.Teams._Shared;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Teams.GetTeamById;

public class AddTeamEndpoint(IMediator mediator, TeamMapper mapper) : Endpoint<AddTeamRequest, TeamResponse>
{

    public override void Configure()
    {
        Post("/api/teams");
        AllowAnonymous();
        Description(b => b
            .Produces<TeamResponse>(201)
            .Produces(400)
            .Produces(409)
            .WithTags("Teams"));
        Summary(s =>
        {
            s.Summary = "Create a new team";
            s.ExampleRequest = AddTeamRequest.Example;
        });
    }

    public override async Task HandleAsync(
        AddTeamRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await this.SendCreatedAtAsync<GetTeamByIdEndpoint>(
            result.Value.Id, result.Value, ct);
    }
}
