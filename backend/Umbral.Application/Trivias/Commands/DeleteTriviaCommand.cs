using MediatR;

namespace Umbral.Application.Trivias.Commands;

public record DeleteTriviaCommand(Guid Id) : IRequest;