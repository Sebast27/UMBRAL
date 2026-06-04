using MediatR;
using Umbral.Application.Common.Interfaces;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class JoinSessionCommandHandler : IRequestHandler<JoinSessionCommand, Guid>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JoinSessionCommandHandler(
        ISessionRepository sessionRepository,
        IParticipantRepository participantRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _participantRepository = participantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            throw new Domain.Common.Exceptions.DomainException("Sesión no encontrada");

        if (session.Status != SessionStatus.Waiting)
            throw new Domain.Common.Exceptions.DomainException("La sesión ya comenzó o finalizó");

        var participantType = request.IsTeamMember ? ParticipantType.TeamMember : ParticipantType.Individual;
        var participant = new Participant(
            request.ParticipantName,
            participantType,
            session.Id,
            request.TeamId);

        await _participantRepository.AddAsync(participant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return participant.Id;
    }
}