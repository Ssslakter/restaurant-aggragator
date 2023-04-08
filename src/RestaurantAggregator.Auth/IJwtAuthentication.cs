using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using RestaurantAggregator.Auth.Data;

namespace RestaurantAggregator.Auth;

public interface IJwtAuthentication
{
    string GenerateToken(IEnumerable<Claim> claims);

    bool ValidateToken(string token, [NotNullWhen(true)] out SecurityToken? validatedToken);
}
