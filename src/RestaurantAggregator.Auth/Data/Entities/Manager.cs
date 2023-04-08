namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class Manager
{
    public User User { get; set; }
    public Guid RestaurantId { get; set; }
}
