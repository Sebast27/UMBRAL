using Umbral.Application.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Infrastructure.Persistence;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class AnswerRepository : IAnswerRepository
{
    private readonly AppDbContext _context;

    public AnswerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Answer answer, CancellationToken ct = default)
    {
        await _context.Answers.AddAsync(answer, ct);
    }
}