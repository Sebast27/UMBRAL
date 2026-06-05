using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, AnswerResultDto>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IGameRoundRepository _roundRepository;
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAnswerCommandHandler(
        ISessionRepository sessionRepository,
        IGameRoundRepository roundRepository,
        ITriviaRepository triviaRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _roundRepository = roundRepository;
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AnswerResultDto> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null || session.Status != SessionStatus.Active)
            return new AnswerResultDto
            {
                IsCorrect = false,
                PointsEarned = 0,
                IsWinner = false,
                Message = "La sesión no está activa"
            };

        var currentRound = await _roundRepository.GetCurrentRoundAsync(request.SessionId, cancellationToken);
        if (currentRound is null)
            return new AnswerResultDto
            {
                IsCorrect = false,
                PointsEarned = 0,
                IsWinner = false,
                Message = "No hay una ronda activa"
            };

        // Verificar si ya respondió
        if (currentRound.Answers.Any(a => a.ParticipantId == request.ParticipantId))
            return new AnswerResultDto
            {
                IsCorrect = false,
                PointsEarned = 0,
                IsWinner = false,
                Message = "Ya has respondido esta pregunta"
            };

        var trivia = await _triviaRepository.GetByIdAsync(session.TriviaId, cancellationToken);
        var question = trivia?.Questions.FirstOrDefault(q => q.Id == currentRound.QuestionId);
        
        var isCorrect = question != null && question.CorrectOption == request.SelectedOption;
        var points = isCorrect && question != null ? question.Points.Value : 0;

        var answer = new Answer(currentRound.Id, request.ParticipantId, request.SelectedOption, request.TimeElapsedMs);
        
        // Agregar la respuesta al repositorio
        await _roundRepository.AddAnswerAsync(answer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AnswerResultDto
        {
            IsCorrect = isCorrect,
            PointsEarned = points,
            IsWinner = false,
            Message = isCorrect ? "¡Respuesta correcta!" : "Respuesta incorrecta"
        };
    }
}