using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser
{
    public string Name { get; private set; } = string.Empty;

    public DateTime RegistrationDate { get; private set; } = DateTime.UtcNow;
}