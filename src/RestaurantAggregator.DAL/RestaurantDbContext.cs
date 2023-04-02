using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data;
#nullable disable
namespace RestaurantAggregator.DAL
{
    internal class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
