namespace RestaurantAggregator.Auth.Client.Services;

public interface IAuthService
{
    Task<string> GetTokenAsync();
}
