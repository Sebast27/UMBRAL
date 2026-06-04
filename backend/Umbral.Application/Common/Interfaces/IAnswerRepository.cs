using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Application.Common.Interfaces;

public interface IAnswerRepository
{
    Task AddAsync(Answer answer, CancellationToken ct = default);
}