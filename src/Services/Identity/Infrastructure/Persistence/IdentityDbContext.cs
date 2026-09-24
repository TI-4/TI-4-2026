using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Admin> Admins => Set<Admin>();

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();

    public DbSet<Docente> Docentes => Set<Docente>();

    public DbSet<FuncionarioObjetosPerdidos> FuncionariosObjetosPerdidos => Set<FuncionarioObjetosPerdidos>();

    public DbSet<FuncionarioQuejas> FuncionariosQuejas => Set<FuncionarioQuejas>();

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Identidad_Rol");

        builder.Entity<Admin>().ToTable("Identidad_Admin");
        builder.Entity<Estudiante>().ToTable("Identidad_Estudiante");
        builder.Entity<Docente>().ToTable("Identidad_Docente");
        builder.Entity<FuncionarioObjetosPerdidos>().ToTable("Identidad_Funcionario_ObjetosPerdidos");
        builder.Entity<FuncionarioQuejas>().ToTable("Identidad_Funcionario_Quejas");

        builder.Entity<Admin>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<Estudiante>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<Docente>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<FuncionarioObjetosPerdidos>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<FuncionarioQuejas>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
    }
}