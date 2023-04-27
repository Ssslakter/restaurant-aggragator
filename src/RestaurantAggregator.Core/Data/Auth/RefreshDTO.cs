namespace RestaurantAggregator.Core.Data.Auth;
#nullable disable
public class RefreshDTO
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
}