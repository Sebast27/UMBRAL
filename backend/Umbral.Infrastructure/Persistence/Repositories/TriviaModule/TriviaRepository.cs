using Microsoft.EntityFrameworkCore;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.Common.Exceptions;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class TriviaRepository : ITriviaRepository
{
    private readonly AppDbContext _context;

    public TriviaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Trivia?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Trivias
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct);
    }

    public async Task<IEnumerable<Trivia>> ListAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.Trivias
            .Include(t => t.Questions)
            .Where(t => !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Name.Contains(search));
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        return await _context.Trivias.AnyAsync(t => t.Name == name && !t.IsDeleted, ct);
    }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await _context.Trivias.CountAsync(t => !t.IsDeleted, ct);
    }

    public async Task AddAsync(Trivia trivia, CancellationToken ct = default)
    {
        await _context.Trivias.AddAsync(trivia, ct);
    }

    public Task UpdateAsync(Trivia trivia, CancellationToken ct = default)
    {
        _context.Entry(trivia).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Trivia trivia, CancellationToken ct = default)
    {
        _context.Trivias.Remove(trivia);
        return Task.CompletedTask;
    }

    public async Task<Trivia?> GetByIdWithTrackingAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Trivias
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct);
    }

    public async Task AddQuestionAsync(Guid triviaId, Question question, CancellationToken ct = default)
    {
        var trivia = await _context.Trivias.FindAsync(new object[] { triviaId }, ct);
        if (trivia == null)
            throw new DomainException("Trivia no encontrada");
        
        await _context.Questions.AddAsync(question, ct);
    }
}