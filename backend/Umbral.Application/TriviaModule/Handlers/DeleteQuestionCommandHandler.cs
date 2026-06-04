using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Application.TriviaModule.Handlers;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(ITriviaRepository triviaRepository, IUnitOfWork unitOfWork)
    {
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var trivia = await _triviaRepository.GetByIdAsync(request.TriviaId, cancellationToken);
        if (trivia is null)
            throw new Domain.Common.Exceptions.DomainException("Trivia no encontrada");

        trivia.RemoveQuestion(request.QuestionId);
        await _triviaRepository.UpdateAsync(trivia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}