using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Auth.Data;
using RestaurantAggregator.Auth;
using RestaurantAggregator.Auth.Data.Enums;
#nullable disable
namespace RestaurantAggregator.Auth.Data;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UsersWithRoles"));

        modelBuilder.Entity<User>()
        .HasAlternateKey(u => u.Email);

        modelBuilder.Entity<Manager>()
            .HasOne(m => m.User);

        modelBuilder.Entity<UserRole>()
        .HasAlternateKey(r => r.Role);

        modelBuilder.Entity<UserRole>()
        .HasAlternateKey(r => r.Name);
    }
}
