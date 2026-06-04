using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record DeleteQuestionCommand(Guid TriviaId, Guid QuestionId) : IRequest;