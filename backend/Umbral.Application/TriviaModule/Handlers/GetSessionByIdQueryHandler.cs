using MediatR;
using Umbral.Application.TriviaModule.Queries;
using Umbral.Domain.TriviaModule.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, Domain.TriviaModule.Entities.Session?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionByIdQueryHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Domain.TriviaModule.Entities.Session?> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _sessionRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}