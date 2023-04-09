namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class Manager
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid RestaurantId { get; set; }
}
