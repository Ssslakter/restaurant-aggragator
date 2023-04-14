namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class Courier
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}
