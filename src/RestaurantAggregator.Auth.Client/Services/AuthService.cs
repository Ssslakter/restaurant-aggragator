using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RestaurantAggregator.Auth.Client.Services;

public class AuthService : IAuthService
{
    private readonly IAuthApiClient _authApiClient;
    private readonly TokenInfo _tokenInfo;
    private readonly ILogger<AuthService> _logger;
    private readonly IOptions<AuthApiSecret> _authApiSecret;

    public AuthService(IAuthApiClient authApiClient,
     IOptions<AuthApiSecret> authApiSecret, ILogger<AuthService> logger, TokenInfo tokenInfo)
    {
        _authApiClient = authApiClient;
        _tokenInfo = tokenInfo;
        _authApiSecret = authApiSecret;
        _logger = logger;
    }

    public async Task Authenticate()
    {
        if (string.IsNullOrEmpty(_tokenInfo.AccessToken))
        {
            await AuthenticateAsync();
            return;
        }
        switch (_tokenInfo.AccessTokenExpiration)
        {
            case var exp when exp > DateTime.UtcNow.AddMinutes(1):
                break;
            default:
                switch (_tokenInfo.RefreshTokenExpiration)
                {
                    case var exp when exp > DateTime.UtcNow.AddMinutes(1):
                        await RefreshAsync();
                        break;
                    default:
                        await AuthenticateAsync();
                        break;
                }
                break;
        }
    }

    private async Task AuthenticateAsync()
    {
        var loginModel = new LoginModel
        {
            Email = _authApiSecret.Value.Email,
            Password = _authApiSecret.Value.Password
        };
        try
        {
            var response = await _authApiClient.LoginAsync(loginModel);
            _tokenInfo.ConfigureTokens(response.AccessToken, response.AccessTokenExpires,
             response.RefreshToken, response.RefreshTokenExpires, response.UserId);
            _authApiClient.SetToken(_tokenInfo.AccessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while authenticating");
            throw;
        }
    }

    private async Task RefreshAsync()
    {
        var tokenModel = new RefreshDTO
        {
            UserId = _tokenInfo.UserId,
            RefreshToken = _tokenInfo.RefreshToken
        };
        try
        {
            var response = await _authApiClient.RefreshAsync(tokenModel);
            _tokenInfo.ConfigureTokens(response.AccessToken, response.AccessTokenExpires,
             response.RefreshToken, response.RefreshTokenExpires, response.UserId);
            _authApiClient.SetToken(_tokenInfo.AccessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refreshing token");
            throw;
        }
    }
}