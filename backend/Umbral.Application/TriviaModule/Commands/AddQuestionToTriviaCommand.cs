using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record AddQuestionToTriviaCommand(
    Guid TriviaId,
    string Text,
    string[] Options,
    int CorrectOption,
    int Points) : IRequest<Guid>;