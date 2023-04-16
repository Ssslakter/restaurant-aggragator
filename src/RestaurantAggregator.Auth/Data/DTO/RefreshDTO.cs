namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class RefreshDTO
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
}