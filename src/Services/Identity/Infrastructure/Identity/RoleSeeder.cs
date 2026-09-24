using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Identity;

public static class RoleSeeder
{
    private static readonly string[] DefaultRoles =
    [
        Role.Admin,
        Role.Student,
        Role.Teacher,
        Role.ObjectsOfficer,
        Role.ComplaintsOfficer
    ];

    public static async Task EnsureAsync(RoleManager<Role> roleManager)
    {
        ArgumentNullException.ThrowIfNull(roleManager);

        foreach (var roleName in DefaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role(roleName));
            }
        }
    }
}
