using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record RemoveParticipantCommand(Guid SessionId, Guid ParticipantId) : IRequest;