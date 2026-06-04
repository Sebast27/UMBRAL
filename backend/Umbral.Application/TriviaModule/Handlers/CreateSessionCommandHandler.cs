using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Guid>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSessionCommandHandler(
        ISessionRepository sessionRepository,
        ITriviaRepository triviaRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        // Verificar que la trivia existe y tiene preguntas
        var trivia = await _triviaRepository.GetByIdAsync(request.TriviaId, cancellationToken);
        if (trivia is null)
            throw new Domain.Common.Exceptions.DomainException("Trivia no encontrada");

        if (!trivia.CanBeUsedInSession())
            throw new Domain.Common.Exceptions.DomainException("La trivia no tiene preguntas para usar en una sesión");

        // Crear la sesión
        var session = new Session(
            request.TriviaId,
            request.TimePerQuestion,
            request.IsPublic);

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}