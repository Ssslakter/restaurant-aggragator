using System.Security.Claims;
using RestaurantAggregator.Auth.Data.Entities;

namespace RestaurantAggregator.Auth.Services;

public interface IJwtAuthentication
{
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);
    AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task RevokeAllUserRefreshTokensAsync(Guid userId);
}