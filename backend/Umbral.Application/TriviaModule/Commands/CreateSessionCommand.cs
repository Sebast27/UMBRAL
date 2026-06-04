using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record CreateSessionCommand(
    Guid TriviaId,
    int TimePerQuestion,
    bool IsPublic) : IRequest<Guid>;