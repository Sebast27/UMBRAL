using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
{
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(ITriviaRepository triviaRepository, IUnitOfWork unitOfWork)
    {
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var trivia = await _triviaRepository.GetByIdAsync(request.TriviaId, cancellationToken);
        if (trivia is null)
            throw new Domain.Common.Exceptions.DomainException("Trivia no encontrada");

        var points = new Points(request.Points);
        trivia.UpdateQuestion(request.QuestionId, request.Text, request.Options, request.CorrectOption, points);
        
        await _triviaRepository.UpdateAsync(trivia, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}