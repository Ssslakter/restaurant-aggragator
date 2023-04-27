namespace RestaurantAggregator.Auth.DAL.Data.Entities;
#nullable disable
public class AccessToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}
