namespace RestaurantAggregator.Auth.DAL.Data.Entities;
#nullable disable
public class Client
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string Address { get; set; }
}
