using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(StartSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            throw new Domain.Common.Exceptions.DomainException("Sesión no encontrada");

        session.Start();
        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}