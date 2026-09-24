namespace IdentityService.Domain.Entities;

public class Estudiante
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string Career { get; private set; } = string.Empty;

    public Guid CampusId { get; private set; }
}