using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class Role : IdentityRole
{
    public const string Admin = "Admin";

    public const string Student = "Student";

    public const string Teacher = "Teacher";

    public const string ObjectsOfficer = "ObjectsOfficer";

    public const string ComplaintsOfficer = "ComplaintsOfficer";

    public Role()
    {
    }

    public Role(string name)
        : base(name)
    {
    }
}
