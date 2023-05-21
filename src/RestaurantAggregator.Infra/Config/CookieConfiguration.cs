namespace RestaurantAggregator.Infra.Config;

public class CookieConfiguration
{
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromMinutes(20);
    public string AccessDeniedPath { get; set; } = "/forbidden/";
    public string Issuer { get; set; } = "http://localhost";
}
