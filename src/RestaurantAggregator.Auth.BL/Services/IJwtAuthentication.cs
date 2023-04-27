using System.Security.Claims;
using RestaurantAggregator.Auth.DAL.Data.Entities;

namespace RestaurantAggregator.Auth.BL.Services;

public interface IJwtAuthentication
{
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);
    AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task RevokeAllUserRefreshTokensAsync(Guid userId);
}