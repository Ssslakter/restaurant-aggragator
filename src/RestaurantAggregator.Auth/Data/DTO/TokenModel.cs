namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class TokenModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public Guid UserId { get; set; }
}
