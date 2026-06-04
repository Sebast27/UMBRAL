using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Application.Common.Interfaces;

public interface IParticipantRepository
{
    Task AddAsync(Participant participant, CancellationToken ct = default);
}