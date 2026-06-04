using Umbral.Application.Common.Interfaces;
using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Infrastructure.Persistence.Repositories.TriviaModule;

public class ParticipantRepository : IParticipantRepository
{
    private readonly AppDbContext _context;

    public ParticipantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Participant participant, CancellationToken ct = default)
    {
        await _context.Participants.AddAsync(participant, ct);
    }
}