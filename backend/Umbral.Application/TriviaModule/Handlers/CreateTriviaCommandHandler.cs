using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Entities;
using Umbral.Domain.Exceptions;
using Umbral.Domain.Repositories;

namespace Umbral.Application.TriviaModule.Handlers;

public class CreateTriviaCommandHandler : IRequestHandler<CreateTriviaCommand, Guid>
{
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTriviaCommandHandler(ITriviaRepository triviaRepository, IUnitOfWork unitOfWork)
    {
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTriviaCommand request, CancellationToken cancellationToken)
    {
        var exists = await _triviaRepository.ExistsByNameAsync(request.Name, cancellationToken);
        if (exists)
            throw new DomainException("Ya existe una trivia con ese nombre");

        var trivia = new Trivia(request.Name, request.Description, request.CreatedBy);
        await _triviaRepository.AddAsync(trivia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trivia.Id;
    }
}