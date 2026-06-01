using MediatR;

namespace Umbral.Application.Trivias.Commands;

public record UpdateTriviaCommand(Guid Id, string Name, string? Description) : IRequest;