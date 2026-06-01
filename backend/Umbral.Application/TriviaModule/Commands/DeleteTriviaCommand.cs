using MediatR;

namespace Umbral.Application.TriviaModule.Commands;

public record DeleteTriviaCommand(Guid Id) : IRequest;