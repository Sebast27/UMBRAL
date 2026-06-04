using MediatR;
using Umbral.Application.Common;
using Umbral.Application.TriviaModule.Dtos;

namespace Umbral.Application.TriviaModule.Queries;

public record ListSessionsQuery(int Page = 1, int PageSize = 10, bool? OnlyPublic = null) 
    : IRequest<PaginatedResult<SessionListItemDto>>;