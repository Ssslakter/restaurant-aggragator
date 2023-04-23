namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class TokenModel
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
    public Guid UserId { get; set; }
}
