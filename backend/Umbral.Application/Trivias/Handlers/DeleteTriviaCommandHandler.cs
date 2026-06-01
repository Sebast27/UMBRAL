using MediatR;
using Umbral.Application.Trivias.Commands;
using Umbral.Domain.Repositories;

namespace Umbral.Application.Trivias.Handlers;

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
            throw new Domain.Exceptions.DomainException("Trivia no encontrada");

        trivia.Delete();
        await _triviaRepository.UpdateAsync(trivia, cancellationToken);
    }
}