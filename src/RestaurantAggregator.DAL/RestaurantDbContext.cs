using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data;
using RestaurantAggregator.DAL.Data;
#nullable disable
namespace RestaurantAggregator.DAL
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DishInCart> DishesInCarts { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasAlternateKey(r => new { r.DishId, r.ClientId });

            modelBuilder.Entity<Order>()
                .Property(r => r.OrderNumber)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                .HasAlternateKey(o => new { o.ClientId, o.OrderTime });

            modelBuilder.Entity<DishInCart>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Dishes)
                .IsRequired(false);

            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Dishes)
                .WithMany();
        }
    }
}
