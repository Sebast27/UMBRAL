using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record JoinSessionCommand(
    Guid SessionId,
    string ParticipantName,
    bool IsTeamMember,
    Guid? TeamId = null) : IRequest<Guid>;