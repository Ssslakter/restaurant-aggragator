using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RestaurantAggregator.Auth.Client.Services;

public class AuthService : IAuthService
{
    private readonly IAuthApiClient _authApiClient;
    private TokenInfo _tokenInfo;
    private readonly ILogger<AuthService> _logger;
    private readonly IOptions<AuthApiSecret> _authApiSecret;

    public AuthService(IAuthApiClient authApiClient, IOptions<AuthApiSecret> authApiSecret, ILogger<AuthService> logger)
    {
        _authApiClient = authApiClient;
        _tokenInfo = new TokenInfo();
        _authApiSecret = authApiSecret;
        _logger = logger;
    }

    public async Task<string> GetTokenAsync()
    {
        if (string.IsNullOrEmpty(_tokenInfo.AccessToken) || _tokenInfo.AccessTokenExpiration < DateTime.UtcNow)
        {
            await AuthenticateAsync();
        }

        return _tokenInfo.AccessToken;
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
            _tokenInfo = _tokenInfo.ConfigureTokens(response.AccessToken, response.RefreshToken, response.UserId);
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
            _tokenInfo = _tokenInfo.ConfigureTokens(response.AccessToken, response.RefreshToken, response.UserId);
        }
        catch (ApiException ex)
        {
            if (ex.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized)
            {
                await AuthenticateAsync();
            }
            else
            {
                _logger.LogError(ex, "Error while refreshing token");
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refreshing token");
            throw;
        }
    }

    public class TokenInfo
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public Guid UserId { get; set; }

        public TokenInfo()
        {
            AccessToken = string.Empty;
            AccessTokenExpiration = DateTime.MinValue;
            RefreshToken = string.Empty;
            RefreshTokenExpiration = DateTime.MinValue;
        }

        public TokenInfo ConfigureTokens(string accessToken, string refreshToken, Guid userId)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            UserId = userId;
            return this;
        }
    }
}