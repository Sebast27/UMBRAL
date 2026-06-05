using MediatR;
using Umbral.Application.TriviaModule.Queries;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class GetCurrentQuestionQueryHandler : IRequestHandler<GetCurrentQuestionQuery, CurrentQuestionDto?>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IGameRoundRepository _roundRepository;
    private readonly ITriviaRepository _triviaRepository;

    public GetCurrentQuestionQueryHandler(
        ISessionRepository sessionRepository,
        IGameRoundRepository roundRepository,
        ITriviaRepository triviaRepository)
    {
        _sessionRepository = sessionRepository;
        _roundRepository = roundRepository;
        _triviaRepository = triviaRepository;
    }

    public async Task<CurrentQuestionDto?> Handle(GetCurrentQuestionQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null || session.Status != SessionStatus.Active)
            return null;

        var currentRound = await _roundRepository.GetCurrentRoundAsync(request.SessionId, cancellationToken);
        if (currentRound is null)
            return null;

        var trivia = await _triviaRepository.GetByIdAsync(session.TriviaId, cancellationToken);
        if (trivia is null)
            return null;

        var question = trivia.Questions.FirstOrDefault(q => q.Id == currentRound.QuestionId);
        if (question is null)
            return null;

        return new CurrentQuestionDto
        {
            QuestionId = question.Id,
            Text = question.Text,
            Options = question.Options,
            TimeLimit = session.TimePerQuestion,
            StartedAt = currentRound.StartedAt
        };
    }
}