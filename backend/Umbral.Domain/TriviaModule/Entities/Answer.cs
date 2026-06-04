namespace Umbral.Domain.TriviaModule.Entities;

public class Answer
{
    public Guid Id { get; private set; }
    public Guid RoundId { get; private set; }
    public Guid ParticipantId { get; private set; }
    public int SelectedOption { get; private set; }
    public int TimeElapsedMs { get; private set; }
    public DateTime AnsweredAt { get; private set; }

    private Answer() { }

    public Answer(Guid roundId, Guid participantId, int selectedOption, int timeElapsedMs)
    {
        Id = Guid.NewGuid();
        RoundId = roundId;
        ParticipantId = participantId;
        SelectedOption = selectedOption;
        TimeElapsedMs = timeElapsedMs;
        AnsweredAt = DateTime.UtcNow;
    }
}