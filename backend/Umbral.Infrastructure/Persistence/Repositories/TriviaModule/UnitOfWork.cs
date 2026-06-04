using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.Common.Interfaces;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}