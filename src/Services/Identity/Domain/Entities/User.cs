using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}