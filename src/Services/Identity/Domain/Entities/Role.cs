using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class Role : IdentityRole
{
    public const string Admin = "Admin";

    public const string Estudiante = "Estudiante";

    public const string Docente = "Docente";

    public const string Funcionario = "Funcionario";

    public Role()
    {
    }

    public Role(string name)
        : base(name)
    {
    }
}