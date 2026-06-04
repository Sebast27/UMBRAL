using Umbral.Domain.Common.Exceptions;

namespace Umbral.Domain.TriviaModule.Entities;

public class GameRound
{
    public Guid Id { get; private set; }
    public Guid SessionId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int RoundNumber { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public Guid? WinnerParticipantId { get; private set; }

    private readonly List<Answer> _answers = new();
    public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

    private GameRound() { }

    public GameRound(Guid sessionId, Guid questionId, int roundNumber)
    {
        Id = Guid.NewGuid();
        SessionId = sessionId;
        QuestionId = questionId;
        RoundNumber = roundNumber;
        StartedAt = DateTime.UtcNow;
    }

    public void AddAnswer(Guid participantId, int selectedOption, int timeElapsedMs)
    {
        var answer = new Answer(Id, participantId, selectedOption, timeElapsedMs);
        _answers.Add(answer);
    }

    public void EndRound(Guid winnerParticipantId)
    {
        if (EndedAt.HasValue)
            throw new DomainException("La ronda ya finalizó");

        WinnerParticipantId = winnerParticipantId;
        EndedAt = DateTime.UtcNow;
    }

    public bool IsCorrectAnswer(int selectedOption)
    {
        // Esto requiere acceso a la pregunta, se resolverá en el handler
        return false;
    }
}