using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Application.TriviaModule.Dtos;

public record SessionListItemDto
{
    public Guid Id { get; init; }
    public Guid TriviaId { get; init; }
    public string TriviaName { get; init; } = string.Empty;
    public SessionCode Code { get; init; } = null!;
    public int TimePerQuestion { get; init; }
    public bool IsPublic { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public int ParticipantsCount { get; init; }
    public DateTime CreatedAt { get; init; }
}