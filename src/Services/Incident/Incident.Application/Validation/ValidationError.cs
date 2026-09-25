namespace Incident.Application.Validation;

/// <summary>
/// Error individual de validación con código estable para el cliente y campo afectado.
/// </summary>
public sealed record ValidationError(string Code, string Field, string Message);
