using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using JWTAuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthServer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserApp, IdentityRole, string>(options)
{
    public DbSet<Product> Produts { get; set; }

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(builder);
    }
}