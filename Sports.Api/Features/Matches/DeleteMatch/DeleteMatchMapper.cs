namespace Sports.Api.Features.Matches.DeleteMatch;

using Riok.Mapperly.Abstractions;

[Mapper]
public partial class DeleteMatchMapper
{
    public partial DeleteMatchCommand ToCommand(DeleteMatchRequest request);
}