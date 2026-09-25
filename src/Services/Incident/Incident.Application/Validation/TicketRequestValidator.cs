using Google.Protobuf.WellKnownTypes;
using Incident.Application.Protos;

namespace Incident.Application.Validation;

/// <summary>
/// Valida el contrato de entrada de un ticket. Cada validación en un método corto.
/// Ejecuta: presencia -> formato -> rango de enum -> contenido -> reglas cruzadas.
/// </summary>
public static class TicketRequestValidator
{
    private const int MaxTitleLength = 200;
    private const int MaxDescriptionLength = 5000;
    private const int MaxObjectIdLength = 24;

    /// <summary>Valida el request y lo devuelve no-nulo, o lanza <see cref="ValidationException"/>.</summary>
    public static CreateTicketRequest Validate(CreateTicketRequest? request)
    {
        if (request is null)
        {
            throw new ValidationException(
            [
                new ValidationError("EMPTY_JSON_BODY", "body", "El cuerpo de la petición está vacío.")
            ]);
        }

        var errors = new List<ValidationError>();

        ValidateRequiredGuid(errors, "INVALID_USER_ID", "user_id", request.UserId);
        ValidateRequiredGuid(errors, "INVALID_STRUCTURE_ID", "structure_id", request.StructureId);
        ValidateTicketType(errors, request.TicketType);
        ValidateDateReport(errors, request.DateReport);
        ValidateLocation(errors, request.Location);
        ValidateObjectId(errors, request);
        ValidateComplaintDetails(errors, request.ComplaintDetails);

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }

        return request;
    }

    private static void ValidateRequiredGuid(List<ValidationError> errors, string code, string field, string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out _))
        {
            errors.Add(new ValidationError(code, field, $"El campo '{field}' debe ser un GUID válido."));
        }
    }

    private static void ValidateTicketType(List<ValidationError> errors, TicketTypeProto value)
    {
        if (value == TicketTypeProto.TicketTypeUnknown || !System.Enum.IsDefined(value))
        {
            errors.Add(new ValidationError(
                "INVALID_TICKET_TYPE",
                "ticket_type",
                "El 'ticket_type' debe ser CLAIM, FOUND, MATCH o PICKUP."));
        }
    }

    private static void ValidateDateReport(List<ValidationError> errors, Timestamp? dateReport)
    {
        if (dateReport is null)
        {
            errors.Add(new ValidationError("INVALID_DATE_REPORT", "date_report", "El campo 'date_report' es obligatorio."));
            return;
        }

        var date = dateReport.ToDateTime();
        var now = DateTime.UtcNow;

        if (date > now.AddMinutes(5))
        {
            errors.Add(new ValidationError("INVALID_DATE_REPORT", "date_report", "El 'date_report' no puede estar en el futuro."));
        }

        if (date < now.AddYears(-2))
        {
            errors.Add(new ValidationError("INVALID_DATE_REPORT", "date_report", "El 'date_report' no puede ser tan antiguo."));
        }
    }

    private static void ValidateLocation(List<ValidationError> errors, GeoPointMessage? location)
    {
        if (location is null)
        {
            return;
        }

        if (!IsInRange(location.Latitude, -90d, 90d))
        {
            errors.Add(new ValidationError("INVALID_LOCATION", "location.latitude", "La latitud debe estar entre -90 y 90."));
        }

        if (!IsInRange(location.Longitude, -180d, 180d))
        {
            errors.Add(new ValidationError("INVALID_LOCATION", "location.longitude", "La longitud debe estar entre -180 y 180."));
        }
    }

    private static void ValidateObjectId(List<ValidationError> errors, CreateTicketRequest request)
    {
        if (!request.HasObjectId)
        {
            return;
        }

        if (!IsValidMongoObjectId(request.ObjectId))
        {
            errors.Add(new ValidationError(
                "INVALID_OBJECT_ID",
                "object_id",
                $"El 'object_id' debe tener {MaxObjectIdLength} caracteres hexadecimales."));
        }
    }

    private static void ValidateComplaintDetails(List<ValidationError> errors, ComplaintDetailsMessage? details)
    {
        if (details is null)
        {
            return;
        }

        ValidateRequiredText(errors, "INVALID_TITLE", "complaint_details.title", details.Title, MaxTitleLength);
        ValidateRequiredText(errors, "INVALID_DESCRIPTION", "complaint_details.description", details.Description, MaxDescriptionLength);
        ValidateStatus(errors, details.Status);
    }

    private static void ValidateRequiredText(List<ValidationError> errors, string code, string field, string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > maxLength)
        {
            errors.Add(new ValidationError(
                code,
                field,
                $"El campo '{field}' es obligatorio y no puede superar los {maxLength} caracteres."));
        }
    }

    private static void ValidateStatus(List<ValidationError> errors, ComplaintStatusProto value)
    {
        if (value == ComplaintStatusProto.ComplaintStatusUnknown || !System.Enum.IsDefined(value))
        {
            errors.Add(new ValidationError(
                "INVALID_STATUS",
                "complaint_details.status",
                "El 'status' debe ser PENDING, IN_PROCESS, RESOLVED o DISMISSED."));
        }
    }

    private static bool IsInRange(double value, double min, double max) =>
        double.IsFinite(value) && value >= min && value <= max;

    private static bool IsValidMongoObjectId(string value) =>
        value.Length == MaxObjectIdLength && value.All(Uri.IsHexDigit);
}