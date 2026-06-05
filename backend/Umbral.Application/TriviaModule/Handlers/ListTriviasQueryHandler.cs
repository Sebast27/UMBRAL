using MediatR;
using Umbral.Application.Common;
using Umbral.Application.TriviaModule.Dtos;
using Umbral.Application.TriviaModule.Queries;
using Umbral.Domain.TriviaModule.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class ListTriviasQueryHandler : IRequestHandler<ListTriviasQuery, PaginatedResult<TriviaListItemDto>>
{
    private readonly ITriviaRepository _triviaRepository;

    public ListTriviasQueryHandler(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public async Task<PaginatedResult<TriviaListItemDto>> Handle(ListTriviasQuery request, CancellationToken cancellationToken)
    {
        var trivias = await _triviaRepository.ListAsync(request.Page, request.PageSize, request.Search, cancellationToken);
        var totalCount = await _triviaRepository.CountAsync(cancellationToken);

        var items = trivias.Select(t => new TriviaListItemDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            QuestionsCount = t.Questions.Count,
            Questions = t.Questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Options = q.Options,
                CorrectOption = q.CorrectOption,
                Points = q.Points.Value
            }).ToList()
        }).ToList();

        return new PaginatedResult<TriviaListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}