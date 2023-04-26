namespace RestaurantAggregator.Auth.Client.Services;

public interface IAuthService
{
    /// <summary>
    /// Must write token data to TokenInfo
    /// </summary>
    Task Authenticate();
}
