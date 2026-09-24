using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<User, Role, string>
{
    public DbSet<AdminProfile> Admins => Set<AdminProfile>();
    public DbSet<StudentProfile> Students => Set<StudentProfile>();
    public DbSet<TeacherProfile> Teachers => Set<TeacherProfile>();
    public DbSet<ObjectsOfficerProfile> ObjectsOfficers => Set<ObjectsOfficerProfile>();
    public DbSet<ComplaintsOfficerProfile> ComplaintsOfficers => Set<ComplaintsOfficerProfile>();

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");

        builder.Entity<AdminProfile>().ToTable("Identidad_Admin");
        builder.Entity<StudentProfile>().ToTable("Identidad_Estudiante");
        builder.Entity<TeacherProfile>().ToTable("Identidad_Docente");
        builder.Entity<ObjectsOfficerProfile>().ToTable("Identidad_Funcionario_ObjetosPerdidos");
        builder.Entity<ComplaintsOfficerProfile>().ToTable("Identidad_Funcionario_Quejas");

        builder.Entity<AdminProfile>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<StudentProfile>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<TeacherProfile>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<ObjectsOfficerProfile>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.Entity<ComplaintsOfficerProfile>().HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
    }
}
