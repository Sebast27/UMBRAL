using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveParticipantCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            throw new Domain.Common.Exceptions.DomainException("Sesión no encontrada");

        session.RemoveParticipant(request.ParticipantId);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}