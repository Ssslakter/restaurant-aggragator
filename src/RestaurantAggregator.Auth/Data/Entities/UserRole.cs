namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class UserRole
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public User User { get; set; }
}