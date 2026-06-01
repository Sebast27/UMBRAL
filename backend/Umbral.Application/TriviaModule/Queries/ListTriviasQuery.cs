using MediatR;
using Umbral.Application.Common;
using Umbral.Application.TriviaModule.Dtos;

namespace Umbral.Application.TriviaModule.Queries;

public record ListTriviasQuery(int Page = 1, int PageSize = 10, string? Search = null) 
    : IRequest<PaginatedResult<TriviaListItemDto>>;