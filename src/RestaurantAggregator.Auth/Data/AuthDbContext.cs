using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RestaurantAggregator.Core.Data.Enums;

#nullable disable
namespace RestaurantAggregator.Auth.Data;

public class AuthDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>().IsRequired();

                entity.HasOne(u => u.Manager)
                .WithOne(c => c.User)
                .HasForeignKey<Manager>().IsRequired();

                entity.HasOne(u => u.Cook)
                .WithOne(c => c.User)
                .HasForeignKey<Cook>().IsRequired();

                entity.HasOne(u => u.Courier)
                .WithOne(c => c.User)
                .HasForeignKey<Courier>().IsRequired();
            });

        modelBuilder.Entity<Role>()
        .HasAlternateKey(r => r.RoleType);

        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(RoleType.Manager),
            NormalizedName = nameof(RoleType.Manager).ToUpper(),
            RoleType = RoleType.Manager
        });
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(RoleType.Cook),
            NormalizedName = nameof(RoleType.Cook).ToUpper(),
            RoleType = RoleType.Cook
        });
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(RoleType.Courier),
            NormalizedName = nameof(RoleType.Courier).ToUpper(),
            RoleType = RoleType.Courier
        });
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(RoleType.Client),
            NormalizedName = nameof(RoleType.Client).ToUpper(),
            RoleType = RoleType.Client
        });
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(RoleType.Admin),
            NormalizedName = nameof(RoleType.Admin).ToUpper(),
            RoleType = RoleType.Admin
        });
    }
}
