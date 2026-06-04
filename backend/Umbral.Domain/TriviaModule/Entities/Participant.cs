using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.TriviaModule.Entities;

public enum ParticipantType
{
    Individual,
    TeamMember
}

public class Participant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public ParticipantType Type { get; private set; }
    public Guid? TeamId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public Guid SessionId { get; private set; }

    private Participant() { }

    public Participant(string name, ParticipantType type, Guid sessionId, Guid? teamId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        SessionId = sessionId;
        TeamId = teamId;
        JoinedAt = DateTime.UtcNow;
    }
}