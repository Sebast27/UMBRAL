using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record UpdateTriviaCommand(Guid Id, string Name, string? Description) : IRequest;