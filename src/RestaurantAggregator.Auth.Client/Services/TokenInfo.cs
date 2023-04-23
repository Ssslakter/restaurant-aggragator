namespace RestaurantAggregator.Auth.Client.Services;
#nullable disable
public class TokenInfo
{
    public string AccessToken { get; private set; }
    public DateTime AccessTokenExpiration { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime RefreshTokenExpiration { get; private set; }
    public Guid UserId { get; private set; }

    public void ConfigureTokens(string accessToken, DateTimeOffset accessTokenExpiration,
     string refreshToken, DateTimeOffset refreshTokenExpiration, Guid userId)
    {
        AccessToken = accessToken;
        AccessTokenExpiration = accessTokenExpiration.UtcDateTime;
        RefreshToken = refreshToken;
        RefreshTokenExpiration = refreshTokenExpiration.UtcDateTime;
        UserId = userId;
    }
}