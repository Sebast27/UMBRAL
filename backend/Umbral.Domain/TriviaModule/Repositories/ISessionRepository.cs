using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.TriviaModule.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Session?> GetByCodeAsync(SessionCode code, CancellationToken ct = default);
    Task<IEnumerable<Session>> ListPublicAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Session session, CancellationToken ct = default);
    Task UpdateAsync(Session session, CancellationToken ct = default);
    Task<IEnumerable<Session>> ListAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAllAsync(CancellationToken ct = default);
    Task<int> CountPublicAsync(CancellationToken ct = default);
}