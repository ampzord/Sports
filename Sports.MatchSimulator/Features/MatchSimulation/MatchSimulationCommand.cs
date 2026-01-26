using MediatR;

namespace Sports.MatchSimulator.Features.MatchSimulation;

public record MatchSimulationCommand : IRequest<MatchSimulationResponse>;