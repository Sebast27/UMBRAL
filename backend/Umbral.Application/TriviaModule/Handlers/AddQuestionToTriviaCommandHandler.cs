using MediatR;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.Common.Exceptions;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Handlers;

public class AddQuestionToTriviaCommandHandler : IRequestHandler<AddQuestionToTriviaCommand, Guid>
{
    private readonly ITriviaRepository _triviaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddQuestionToTriviaCommandHandler(ITriviaRepository triviaRepository, IUnitOfWork unitOfWork)
    {
        _triviaRepository = triviaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddQuestionToTriviaCommand request, CancellationToken cancellationToken)
    {
        var points = new Points(request.Points);
        var question = new Question(request.Text, request.Options, request.CorrectOption, points, request.TriviaId);
        
        await _triviaRepository.AddQuestionAsync(request.TriviaId, question, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return question.Id;
    }
}