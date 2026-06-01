using MediatR;

namespace Umbral.Application.Trivias.Commands;

public record CreateTriviaCommand(string Name, string? Description, Guid CreatedBy) : IRequest<Guid>;