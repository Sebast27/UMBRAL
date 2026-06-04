using MediatR;

namespace Umbral.Application.TriviaModule.Queries;

public record GetCurrentQuestionQuery(Guid SessionId) : IRequest<CurrentQuestionDto?>;

public record CurrentQuestionDto
{
    public Guid QuestionId { get; init; }
    public string Text { get; init; } = string.Empty;
    public string[] Options { get; init; } = Array.Empty<string>();
    public int TimeLimit { get; init; }
    public DateTime StartedAt { get; init; }
}