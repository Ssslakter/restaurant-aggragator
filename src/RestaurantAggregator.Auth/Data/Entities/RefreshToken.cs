namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
}
