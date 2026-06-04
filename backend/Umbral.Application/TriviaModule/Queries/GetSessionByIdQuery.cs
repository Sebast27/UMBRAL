using MediatR;
using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Application.TriviaModule.Queries;

public record GetSessionByIdQuery(Guid Id) : IRequest<Session?>;