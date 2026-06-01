using Umbral.Domain.Entities;

namespace Umbral.Domain.Repositories;

public interface ITriviaRepository
{
    Task<Trivia?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Trivia>> ListAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task AddAsync(Trivia trivia, CancellationToken ct = default);
    Task UpdateAsync(Trivia trivia, CancellationToken ct = default);
    Task DeleteAsync(Trivia trivia, CancellationToken ct = default);
}