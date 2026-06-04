using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record LeaveSessionCommand(Guid SessionId, Guid ParticipantId) : IRequest;