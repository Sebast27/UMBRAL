using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.Common.Exceptions;

namespace Umbral.Application.TriviaModule.Handlers;

public class UpdateTriviaCommandHandler : IRequestHandler<UpdateTriviaCommand>
{
    private readonly ITriviaRepository _triviaRepository;

    public UpdateTriviaCommandHandler(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public async Task Handle(UpdateTriviaCommand request, CancellationToken cancellationToken)
    {
        var trivia = await _triviaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (trivia is null)
            throw new DomainException("Trivia no encontrada");

        trivia.Update(request.Name, request.Description);
        await _triviaRepository.UpdateAsync(trivia, cancellationToken);
    }
}