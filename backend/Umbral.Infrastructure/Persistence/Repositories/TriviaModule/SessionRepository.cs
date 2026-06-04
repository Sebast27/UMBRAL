using Microsoft.EntityFrameworkCore;
using Umbral.Domain.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Sessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Session?> GetByCodeAsync(SessionCode code, CancellationToken ct = default)
    {
        return await _context.Sessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Code == code, ct);
    }

    public async Task<IEnumerable<Session>> ListPublicAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _context.Sessions
            .Where(s => s.IsPublic && s.Status == SessionStatus.Waiting)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Session session, CancellationToken ct = default)
    {
        await _context.Sessions.AddAsync(session, ct);
    }

    public Task UpdateAsync(Session session, CancellationToken ct = default)
    {
        _context.Entry(session).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Session>> ListAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _context.Sessions
            .Include(s => s.Participants)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAllAsync(CancellationToken ct = default)
    {
        return await _context.Sessions.CountAsync(ct);
    }

    public async Task<int> CountPublicAsync(CancellationToken ct = default)
    {
        return await _context.Sessions.CountAsync(s => s.IsPublic && s.Status == SessionStatus.Waiting, ct);
    }
}