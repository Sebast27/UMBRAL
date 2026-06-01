using MediatR;
using Umbral.Application.Trivias.Commands;
using Umbral.Domain.Entities;
using Umbral.Domain.Repositories;

namespace Umbral.Application.Trivias.Handlers;

public class CreateTriviaCommandHandler : IRequestHandler<CreateTriviaCommand, Guid>
{
    private readonly ITriviaRepository _triviaRepository;

    public CreateTriviaCommandHandler(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public async Task<Guid> Handle(CreateTriviaCommand request, CancellationToken cancellationToken)
    {
        var exists = await _triviaRepository.ExistsByNameAsync(request.Name, cancellationToken);
        if (exists)
            throw new Domain.Exceptions.DomainException("Ya existe una trivia con ese nombre");

        var trivia = new Trivia(request.Name, request.Description, request.CreatedBy);
        await _triviaRepository.AddAsync(trivia, cancellationToken);

        return trivia.Id;
    }
}