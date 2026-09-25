using IdentityService.Application.Authentication;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Authentication;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDb")
            ?? throw new InvalidOperationException("Connection string 'IdentityDb' was not found.");

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection("JwtConfig"));

        services.AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<IdentityDbContext>();

        services.AddScoped<IUserAuthenticator, IdentityUserAuthenticator>();
        services.AddScoped<IUserRegistrar, IdentityUserRegistrar>();
        services.AddScoped<IUserQuery, IdentityUserQuery>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();

        return services;
    }
}