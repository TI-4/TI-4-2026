using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using IdentityDbContextBase = Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext;

namespace IdentityService.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContextBase
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }
}