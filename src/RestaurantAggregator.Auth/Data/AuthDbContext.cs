using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
#nullable disable
namespace RestaurantAggregator.Auth;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {

    }
}
