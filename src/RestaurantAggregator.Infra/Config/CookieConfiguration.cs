namespace RestaurantAggregator.Infra.Config;

public class CookieConfiguration
{
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromMinutes(20);
    public string AccessDeniedPath { get; set; } = "/Forbidden/";
    public string Issuer { get; set; } = "http://localhost";
    public string LoginPath { get; set; } = "/Login/";
    public string LogoutPath { get; set; } = "/Logout/";
}
