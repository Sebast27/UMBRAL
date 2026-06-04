using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record SubmitAnswerCommand(
    Guid SessionId,
    Guid ParticipantId,
    int SelectedOption,
    int TimeElapsedMs) : IRequest<AnswerResultDto>;

public record AnswerResultDto
{
    public bool IsCorrect { get; init; }
    public int PointsEarned { get; init; }
    public bool IsWinner { get; init; }
    public string Message { get; init; } = string.Empty;
}