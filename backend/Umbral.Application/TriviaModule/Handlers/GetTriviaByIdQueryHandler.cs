using MediatR;
using Umbral.Application.TriviaModule.Queries;
using Umbral.Domain.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class GetTriviaByIdQueryHandler : IRequestHandler<GetTriviaByIdQuery, Domain.Entities.Trivia?>
{
    private readonly ITriviaRepository _triviaRepository;

    public GetTriviaByIdQueryHandler(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public async Task<Domain.Entities.Trivia?> Handle(GetTriviaByIdQuery request, CancellationToken cancellationToken)
    {
        return await _triviaRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}