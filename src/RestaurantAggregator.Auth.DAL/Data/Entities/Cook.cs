namespace RestaurantAggregator.Auth.DAL.Data.Entities;
#nullable disable
public class Cook
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}
