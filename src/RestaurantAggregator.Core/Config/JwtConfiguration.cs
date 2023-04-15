namespace RestaurantAggregator.Core.Config;
#nullable disable

public interface IJwtConfiguration
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan AccessTokenLifetime { get; set; }
    public TimeSpan RefreshTokenLifetime { get; set; }
}

public class JwtConfiguration : IJwtConfiguration
{
    public string Secret { get; set; } = "RestaurantAggregator";
    public string Issuer { get; set; } = "RestaurantAggregator";
    public string Audience { get; set; } = "MySecretVeryVeryLongKey123!!";
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(20);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(7);
}
