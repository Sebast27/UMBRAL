using Umbral.Domain.Common.Exceptions;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.TriviaModule.Entities;

public class Session
{
    public Guid Id { get; private set; }
    public Guid TriviaId { get; private set; }
    public SessionCode Code { get; private set; }
    public int TimePerQuestion { get; private set; }
    public bool IsPublic { get; private set; }
    public SessionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }

    public List<Participant> Participants { get; private set; } = new();

    private Session() 
    {
        Participants = new List<Participant>();
    }

    public Session(Guid triviaId, int timePerQuestion, bool isPublic)
    {
        if (timePerQuestion < 5 || timePerQuestion > 120)
            throw new DomainException("El tiempo por pregunta debe estar entre 5 y 120 segundos");

        Id = Guid.NewGuid();
        TriviaId = triviaId;
        Code = new SessionCode();
        TimePerQuestion = timePerQuestion;
        IsPublic = isPublic;
        Status = SessionStatus.Waiting;
        CreatedAt = DateTime.UtcNow;
        Participants = new List<Participant>();
    }

    public void AddParticipant(Participant participant)
    {
        if (Status != SessionStatus.Waiting)
            throw new DomainException("No se pueden agregar participantes a una sesión que ya comenzó");

        Participants.Add(participant);
    }

    public void RemoveParticipant(Guid participantId)
    {
        var participant = Participants.FirstOrDefault(p => p.Id == participantId);
        if (participant is null)
            throw new DomainException("Participante no encontrado");

        Participants.Remove(participant);
    }

    public void Start()
    {
        if (Status != SessionStatus.Waiting)
            throw new DomainException("La sesión ya fue iniciada o finalizada");

        if (!Participants.Any())
            throw new DomainException("No hay participantes en la sesión");

        Status = SessionStatus.Active;
        StartedAt = DateTime.UtcNow;
    }

    public void Finish()
    {
        if (Status != SessionStatus.Active)
            throw new DomainException("La sesión no está activa");

        Status = SessionStatus.Finished;
        FinishedAt = DateTime.UtcNow;
    }
}