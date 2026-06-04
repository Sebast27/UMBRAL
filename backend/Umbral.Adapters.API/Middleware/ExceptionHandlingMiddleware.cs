using System.Net;
using System.Text.Json;
using Umbral.Domain.Common.Exceptions;

namespace Umbral.Adapters.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error no controlado");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = exception.Message,
            statusCode = exception is DomainException 
                ? (int)HttpStatusCode.BadRequest 
                : (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = response.statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}