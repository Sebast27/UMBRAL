using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.Common.Exceptions;

namespace Umbral.Application.TriviaModule.Handlers;

public class DeleteTriviaCommandHandler : IRequestHandler<DeleteTriviaCommand>
{
    private readonly ITriviaRepository _triviaRepository;

    public DeleteTriviaCommandHandler(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public async Task Handle(DeleteTriviaCommand request, CancellationToken cancellationToken)
    {
        var trivia = await _triviaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (trivia is null)
            throw new DomainException("Trivia no encontrada");

        trivia.Delete();
        await _triviaRepository.UpdateAsync(trivia, cancellationToken);
    }
}