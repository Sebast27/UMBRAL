using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Domain.TriviaModule.Repositories;

public interface IGameRoundRepository
{
    Task<GameRound?> GetCurrentRoundAsync(Guid sessionId, CancellationToken ct = default);
    Task AddAsync(GameRound round, CancellationToken ct = default);
    //Task UpdateAsync(GameRound round, CancellationToken ct = default);
    Task AddAnswerAsync(Answer answer, CancellationToken ct = default);
}

