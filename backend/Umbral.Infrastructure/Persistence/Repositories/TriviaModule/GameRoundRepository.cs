using Microsoft.EntityFrameworkCore;
using Umbral.Application.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class GameRoundRepository : IGameRoundRepository
{
    private readonly AppDbContext _context;

    public GameRoundRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GameRound?> GetCurrentRoundAsync(Guid sessionId, CancellationToken ct = default)
    {
        return await _context.Set<GameRound>()
            .Include(r => r.Answers)
            .Where(r => r.SessionId == sessionId && !r.EndedAt.HasValue)
            .OrderByDescending(r => r.RoundNumber)
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(GameRound round, CancellationToken ct = default)
    {
        await _context.Set<GameRound>().AddAsync(round, ct);
    }

    public Task UpdateAsync(GameRound round, CancellationToken ct = default)
    {
        _context.Entry(round).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task AddAnswerAsync(Answer answer, CancellationToken ct = default)
    {
        await _context.Answers.AddAsync(answer, ct);
    }
}