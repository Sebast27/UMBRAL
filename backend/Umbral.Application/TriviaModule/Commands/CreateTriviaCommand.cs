using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record CreateTriviaCommand(string Name, string? Description, Guid CreatedBy) : IRequest<Guid>;