namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class Cook
{
    public User User { get; set; }
    public Guid RestaurantId { get; set; }
}
