namespace Incident.Application.Validation;

/// <summary>
/// Excepción lanzada cuando la solicitud no pasa las validaciones de la aplicación.
/// El transporte (REST/gRPC) la traduce a 400 BadRequest / InvalidArgument.
/// </summary>
public sealed class ValidationException : Exception
{
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationException(IReadOnlyList<ValidationError> errors)
        : base("La solicitud no pasó las validaciones.")
    {
        Errors = errors;
    }
}