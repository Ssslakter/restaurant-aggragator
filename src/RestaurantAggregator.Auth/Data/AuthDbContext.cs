using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Auth.Data;
using RestaurantAggregator.Auth;
#nullable disable
namespace RestaurantAggregator.Auth.Data;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {

    }
}
