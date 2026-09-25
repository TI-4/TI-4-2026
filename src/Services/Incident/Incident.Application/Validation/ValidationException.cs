namespace Incident.Application.Validation;


public sealed class ValidationException : Exception
{
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationException(IReadOnlyList<ValidationError> errors)
        : base("La solicitud no pasó las validaciones.")
    {
        Errors = errors;
    }
}
