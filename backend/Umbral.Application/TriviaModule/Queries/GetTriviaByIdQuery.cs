using MediatR;

namespace Umbral.Application.TriviaModule.Queries;

public record GetTriviaByIdQuery(Guid Id) : IRequest<Umbral.Domain.TriviaModule.Entities.Trivia?>;