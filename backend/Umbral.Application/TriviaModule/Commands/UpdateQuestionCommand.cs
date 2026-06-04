using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record UpdateQuestionCommand(
    Guid QuestionId,
    Guid TriviaId,
    string Text,
    string[] Options,
    int CorrectOption,
    int Points) : IRequest;