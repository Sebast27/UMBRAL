using MediatR;
using Umbral.Application.Common;
using Umbral.Application.TriviaModule.Dtos;
using Umbral.Application.TriviaModule.Queries;
using Umbral.Domain.TriviaModule.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class ListSessionsQueryHandler : IRequestHandler<ListSessionsQuery, PaginatedResult<SessionListItemDto>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITriviaRepository _triviaRepository;

    public ListSessionsQueryHandler(ISessionRepository sessionRepository, ITriviaRepository triviaRepository)
    {
        _sessionRepository = sessionRepository;
        _triviaRepository = triviaRepository;
    }

    public async Task<PaginatedResult<SessionListItemDto>> Handle(ListSessionsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.TriviaModule.Entities.Session> sessions;
        int totalCount;

        if (request.OnlyPublic == true)
        {
            sessions = await _sessionRepository.ListPublicAsync(request.Page, request.PageSize, cancellationToken);
            totalCount = await _sessionRepository.CountPublicAsync(cancellationToken);
        }
        else
        {
            sessions = await _sessionRepository.ListAllAsync(request.Page, request.PageSize, cancellationToken);
            totalCount = await _sessionRepository.CountAllAsync(cancellationToken);
        }

        // Obtener nombres de las trivias
        var triviaIds = sessions.Select(s => s.TriviaId).Distinct();
        var trivias = new Dictionary<Guid, string>();
        
        foreach (var triviaId in triviaIds)
        {
            var trivia = await _triviaRepository.GetByIdAsync(triviaId, cancellationToken);
            if (trivia != null)
                trivias[triviaId] = trivia.Name;
        }

        var items = sessions.Select(s => new SessionListItemDto
        {
            Id = s.Id,
            TriviaId = s.TriviaId,
            TriviaName = trivias.GetValueOrDefault(s.TriviaId, "Desconocida"),
            Code = s.Code,
            TimePerQuestion = s.TimePerQuestion,
            IsPublic = s.IsPublic,
            StatusName = s.Status.ToString(),
            ParticipantsCount = s.Participants.Count,
            CreatedAt = s.CreatedAt
        }).ToList();

        return new PaginatedResult<SessionListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}