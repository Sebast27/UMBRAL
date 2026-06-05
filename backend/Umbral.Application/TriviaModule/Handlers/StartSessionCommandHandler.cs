using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITriviaRepository _triviaRepository;
    private readonly IGameRoundRepository _roundRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartSessionCommandHandler(
        ISessionRepository sessionRepository,
        ITriviaRepository triviaRepository,
        IGameRoundRepository roundRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _triviaRepository = triviaRepository;
        _roundRepository = roundRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(StartSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            throw new Domain.Common.Exceptions.DomainException("Sesión no encontrada");

        session.Start();

        var trivia = await _triviaRepository.GetByIdAsync(session.TriviaId, cancellationToken);
        if (trivia is null || !trivia.Questions.Any())
            throw new Domain.Common.Exceptions.DomainException("La trivia no tiene preguntas");

        // Crear la primera ronda con la primera pregunta
        var firstQuestion = trivia.Questions.First();
        var firstRound = new GameRound(session.Id, firstQuestion.Id, 1);
        
        await _roundRepository.AddAsync(firstRound, cancellationToken);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}