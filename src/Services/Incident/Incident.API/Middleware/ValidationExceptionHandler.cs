using Incident.Application.Validation;
using Microsoft.AspNetCore.Diagnostics;

namespace Incident.API.Middleware;

/// <summary>
/// Traduce <see cref="ValidationException"/> a 400 Bad Request con el formato de error estándar.
/// Cualquier otra excepción se deja pasar al comportamiento por defecto (500).
/// </summary>
public sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validation)
        {
            return false;
        }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(ToResponse(validation, context), cancellationToken: cancellationToken);

        return true;
    }

    private static object ToResponse(ValidationException exception, HttpContext context)
    {
        var first = exception.Errors[0];

        return new
        {
            code = "VALIDATION_ERROR",
            message = first.Message,
            requestId = context.TraceIdentifier,
            errors = exception.Errors.Select(e => new { e.Code, e.Field, e.Message })
        };
    }
}