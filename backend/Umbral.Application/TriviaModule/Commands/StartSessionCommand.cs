using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record StartSessionCommand(Guid SessionId) : IRequest;