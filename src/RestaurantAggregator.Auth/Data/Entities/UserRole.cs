using RestaurantAggregator.Auth.Data.Enums;

namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class UserRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
    public ICollection<User> Users { get; set; }
}